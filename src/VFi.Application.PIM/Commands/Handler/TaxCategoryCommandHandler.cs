using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class TaxCategoryCommandHandler : CommandHandler, IRequestHandler<TaxCategoryAddCommand, ValidationResult>,
                                                                IRequestHandler<TaxCategoryDeleteCommand, ValidationResult>,
                                                                IRequestHandler<TaxCategoryEditCommand, ValidationResult>,
                                                                IRequestHandler<TaxCategorySortCommand, ValidationResult>
    {
        private readonly ITaxCategoryRepository _taxCategoryRepository;
        private readonly IProductRepository _productRepository;

        public TaxCategoryCommandHandler(ITaxCategoryRepository TaxCategoryRepository, IProductRepository productRepository)
        {
            _taxCategoryRepository = TaxCategoryRepository;
            _productRepository = productRepository;
        }
        public void Dispose()
        {
            _taxCategoryRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(TaxCategoryAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_taxCategoryRepository)) return request.ValidationResult;
            var taxCategory = new TaxCategory
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Group = request.Group,
                Rate = request.Rate,
                Description = request.Description,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                Type = request.Type,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName
            };

            //add domain event
            //taxCategory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _taxCategoryRepository.Add(taxCategory);
            return await Commit(_taxCategoryRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(TaxCategoryDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_taxCategoryRepository)) return request.ValidationResult;

            var filter = new Dictionary<string, object> { { "taxCategoryId", request.Id } };

            var products = await _productRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var taxCategory = new TaxCategory
            {
                Id = request.Id
            };

            _taxCategoryRepository.Remove(taxCategory);
            return await Commit(_taxCategoryRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(TaxCategoryEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_taxCategoryRepository)) return request.ValidationResult;
            var data = await _taxCategoryRepository.GetById(request.Id);

            var taxCategory = new TaxCategory
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Group = request.Group,
                Rate = request.Rate,
                Description = request.Description,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                Type = request.Type,
                CreatedBy = data.CreatedBy,
                CreatedDate = data.CreatedDate,
                CreatedByName = data.CreatedByName,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate,
                UpdatedByName = request.UpdatedByName
            };

            //add domain event
            //taxCategory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _taxCategoryRepository.Update(taxCategory);
            return await Commit(_taxCategoryRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(TaxCategorySortCommand request, CancellationToken cancellationToken)
        {
            var data = await _taxCategoryRepository.GetAll();

            List<TaxCategory> list = new List<TaxCategory>();

            foreach (var sort in request.SortList)
            {
                TaxCategory obj = data.FirstOrDefault(c => c.Id == sort.Id);
                if (obj != null)
                {
                    obj.DisplayOrder = sort.SortOrder;
                    list.Add(obj);
                }
            }
            _taxCategoryRepository.Update(list);
            return await Commit(_taxCategoryRepository.UnitOfWork);
        }
    }
}

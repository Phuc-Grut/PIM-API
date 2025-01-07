using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands
{
    public class CategoryRootCommandHandler : CommandHandler, IRequestHandler<CategoryRootAddCommand, ValidationResult>,
                                                                IRequestHandler<CategoryRootDeleteCommand, ValidationResult>,
                                                                IRequestHandler<CategoryRootEditCommand, ValidationResult>,
                                                                IRequestHandler<CategoryRootSortCommand, ValidationResult>
    {
        private readonly ICategoryRootRepository _categoryRootRepository;
        private readonly IProductRepository _productRepository;

        public CategoryRootCommandHandler(ICategoryRootRepository categoryRootRepository, IProductRepository productRepository)
        {
            _categoryRootRepository = categoryRootRepository;
            _productRepository = productRepository;
        }
        public void Dispose()
        {
            _categoryRootRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(CategoryRootAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_categoryRootRepository)) return request.ValidationResult;
            var cateRoot = new CategoryRoot
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Url = request.Url,
                Image = request.Image,
                ParentCategoryId = request.ParentCategoryId,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                IdNumber = request.IdNumber,
                Keywords = request.Keywords,
                JsonData = request.JsonData,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName
            };

            //add domain event
            //category.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _categoryRootRepository.Add(cateRoot);
            return await Commit(_categoryRootRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(CategoryRootDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_categoryRootRepository)) return request.ValidationResult;
            var cateRoot = new CategoryRoot
            {
                Id = request.Id
            };

            var filter = new Dictionary<string, object> { { "categoryRootId", request.Id } };

            var products = await _productRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            _categoryRootRepository.Remove(cateRoot);
            return await Commit(_categoryRootRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(CategoryRootEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_categoryRootRepository)) return request.ValidationResult;
            var dataCate = await _categoryRootRepository.GetById(request.Id);


            dataCate.Code = request.Code;
            dataCate.Name = request.Name;
            dataCate.Description = request.Description;
            dataCate.Image = request.Image;
            dataCate.Url = request.Url;
            dataCate.ParentCategoryId = request.ParentCategoryId;
            dataCate.Status = request.Status;
            dataCate.DisplayOrder = request.DisplayOrder;
            dataCate.Keywords = request.Keywords;
            dataCate.JsonData = request.JsonData;
            dataCate.UpdatedBy = request.UpdatedBy;
            dataCate.UpdatedDate = request.UpdatedDate;
            dataCate.UpdatedByName = request.UpdatedByName;
            

            //add domain event
            //category.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _categoryRootRepository.Update(dataCate);
            return await Commit(_categoryRootRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(CategoryRootSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _categoryRootRepository.GetAll();

            List<CategoryRoot> stores = new List<CategoryRoot>();

            foreach (var sort in request.SortList)
            {
                CategoryRoot store = data.FirstOrDefault(c => c.Id == sort.Id);
                if (store != null)
                {
                    store.DisplayOrder = sort.SortOrder;
                    stores.Add(store);
                }
            }
            _categoryRootRepository.Update(stores);
            return await Commit(_categoryRootRepository.UnitOfWork);
        }
    }
}

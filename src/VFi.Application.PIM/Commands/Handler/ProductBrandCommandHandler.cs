using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands.Handler
{
    internal class ProductBrandCommandHandler : CommandHandler, IRequestHandler<ProductBrandAddCommand, ValidationResult>,
                                                                IRequestHandler<ProductBrandDeleteCommand, ValidationResult>,
                                                                IRequestHandler<ProductBrandEditCommand, ValidationResult>,
                                                                IRequestHandler<ProductBrandSortCommand, ValidationResult>
    {
        private readonly IProductBrandRepository _productBrandRepository;
        private readonly IProductRepository _productRepository;

        public ProductBrandCommandHandler(IProductBrandRepository productBrandRepository, IProductRepository productRepository)
        {
            _productBrandRepository = productBrandRepository;
            _productRepository = productRepository;
        }
        public void Dispose()
        {
            _productBrandRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductBrandAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productBrandRepository)) return request.ValidationResult;
            var productBrand = new ProductBrand
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Image = request.Image,
                Tags = request.Tags,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName,
            };

            //add domain event
            //productBrand.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productBrandRepository.Add(productBrand);
            return await Commit(_productBrandRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductBrandDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productBrandRepository)) return request.ValidationResult;

            var filter = new Dictionary<string, object> { { "brandId", request.Id } };

            var products = await _productRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var productBrand = new ProductBrand
            {
                Id = request.Id
            };

            _productBrandRepository.Remove(productBrand);
            return await Commit(_productBrandRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductBrandEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productBrandRepository)) return request.ValidationResult;
            var data = await _productBrandRepository.GetById(request.Id);
            var productBrand = new ProductBrand
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Image = request.Image,
                Tags = request.Tags,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = data.CreatedBy,
                CreatedDate = data.CreatedDate,
                CreatedByName = data.CreatedByName,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate,
                UpdatedByName = request.UpdatedByName
            };

            //add domain event
            //productBrand.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productBrandRepository.Update(productBrand);
            return await Commit(_productBrandRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductBrandSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _productBrandRepository.GetAll();

            List<ProductBrand> productBrands = new List<ProductBrand>();

            foreach (var sort in request.SortList)
            {
                ProductBrand productBrand = data.FirstOrDefault(c => c.Id == sort.Id);
                if (productBrand != null)
                {
                    productBrand.DisplayOrder = sort.SortOrder;
                    productBrands.Add(productBrand);
                }
            }
            _productBrandRepository.Update(productBrands);
            return await Commit(_productBrandRepository.UnitOfWork);
        }
    }
}

using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands.Handler
{
    internal class ProductTypeCommandHandler : CommandHandler, IRequestHandler<ProductTypeAddCommand, ValidationResult>,
                                                               IRequestHandler<ProductTypeDeleteCommand, ValidationResult>,
                                                               IRequestHandler<ProductTypeEditCommand, ValidationResult>,
                                                               IRequestHandler<ProductTypeSortCommand, ValidationResult>

    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductRepository _productRepository;

        public ProductTypeCommandHandler(IProductTypeRepository productTypeRepository, IProductRepository productRepository)
        {
            _productTypeRepository = productTypeRepository;
            _productRepository = productRepository;
        }
        public void Dispose()
        {
            _productTypeRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductTypeAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productTypeRepository)) return request.ValidationResult;
            var productTypes = new ProductType
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName
            };


            _productTypeRepository.Add(productTypes);
            return await Commit(_productTypeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTypeDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productTypeRepository)) return request.ValidationResult;

            var filter = new Dictionary<string, object> { { "productTypeId", request.Id } };

            var products = await _productRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var productTypes = new ProductType
            {
                Id = request.Id
            };


            _productTypeRepository.Remove(productTypes);
            return await Commit(_productTypeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTypeEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productTypeRepository)) return request.ValidationResult;
            var productTypes = await _productTypeRepository.GetById(request.Id);
             
            productTypes.Code = request.Code;
            productTypes.Name = request.Name;
            productTypes.Description = request.Description;
            productTypes.Status = request.Status;
            productTypes.DisplayOrder = request.DisplayOrder;
            productTypes.UpdatedBy = request.UpdatedBy;
            productTypes.UpdatedDate = request.UpdatedDate;
            productTypes.UpdatedByName = request.UpdatedByName;
             
            _productTypeRepository.Update(productTypes);
            return await Commit(_productTypeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTypeSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _productTypeRepository.GetAll();

            List<ProductType> list = new List<ProductType>();

            foreach (var sort in request.SortList)
            {
                ProductType obj = data.FirstOrDefault(c => c.Id == sort.Id);
                if (obj != null)
                {
                    obj.DisplayOrder = sort.SortOrder;
                    list.Add(obj);
                }
            }
            _productTypeRepository.Update(list);
            return await Commit(_productTypeRepository.UnitOfWork);
        }
    }
}

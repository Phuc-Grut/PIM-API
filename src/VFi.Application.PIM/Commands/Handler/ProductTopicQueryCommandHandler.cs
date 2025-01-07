using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductTopicQueryCommandHandler : CommandHandler,
        IRequestHandler<AddProductTopicQueryCommand, ValidationResult>,
        IRequestHandler<DeleteProductTopicQueryCommand, ValidationResult>,
        IRequestHandler<EditProductTopicQueryCommand, ValidationResult>,
        IRequestHandler<ProductTopicQuerySortCommand, ValidationResult>
    {
        private readonly IProductTopicQueryRepository _repository;

        public ProductTopicQueryCommandHandler(IProductTopicQueryRepository repository)
        {
            _repository = repository;
        }
        public void Dispose()
        {
            _repository.Dispose();
        }

        public async Task<ValidationResult> Handle(AddProductTopicQueryCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_repository)) return request.ValidationResult;
            var item = new ProductTopicQuery
            {
                Id = request.Id,
                ProductTopicId = request.ProductTopicId,
                Name = request.Name,
                Title = request.Title,
                Description = request.Description,
                SourceCode = request.SourceCode,
                SourcePath = request.SourcePath,
                Keyword = request.Keyword,
                Category = request.Category,
                Seller = request.Seller,
                BrandId = request.BrandId,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedDate = request.CreatedDate,
                CreatedBy = request.CreatedBy,
                CreatedByName = request.CreatedByName,
                Condition = request.Condition,
                ProductType = request.ProductType,
                PageQuery = request.PageQuery,
                SortQuery = request.SortQuery,
            };

            //add domain event
            //item.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _repository.Add(item);
            return await Commit(_repository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(DeleteProductTopicQueryCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_repository)) return request.ValidationResult;
            var item = new ProductTopicQuery
            {
                Id = request.Id
            };

            //add domain event
            //item.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _repository.Remove(item);
            return await Commit(_repository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(EditProductTopicQueryCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_repository)) return request.ValidationResult;
            var item = await _repository.GetById(request.Id);

            item.ProductTopicId = request.ProductTopicId;
            item.Name = request.Name;
            item.Title = request.Title;
            item.Description = request.Description;
            item.SourceCode = request.SourceCode;
            item.SourcePath = request.SourcePath;
            item.Keyword = request.Keyword;
            item.Category = request.Category;
            item.Seller = request.Seller;
            item.BrandId = request.BrandId;
            item.Status = request.Status;
            item.DisplayOrder = request.DisplayOrder;
            item.UpdatedDate = request.UpdatedDate;
            item.UpdatedBy = request.UpdatedBy;
            item.UpdatedByName = request.UpdatedByName;
            item.Condition = request.Condition;
            item.ProductType = request.ProductType;
            item.PageQuery = request.PageQuery;
            item.SortQuery = request.SortQuery;

            _repository.Update(item);
            return await Commit(_repository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTopicQuerySortCommand request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAll();

            List<ProductTopicQuery> list = new List<ProductTopicQuery>();

            foreach (var sort in request.SortList)
            {
                ProductTopicQuery obj = data.FirstOrDefault(c => c.Id == sort.Id);
                if (obj != null)
                {
                    obj.DisplayOrder = sort.SortOrder;
                    list.Add(obj);
                }
            }
            _repository.Update(list);
            return await Commit(_repository.UnitOfWork);
        }
    }
}

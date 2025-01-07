using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductTopicPageCommandHandler : CommandHandler,
        IRequestHandler<ProductTopicPageAddCommand, ValidationResult>,
        IRequestHandler<ProductTopicPageDeleteCommand, ValidationResult>,
        IRequestHandler<ProductTopicPageEditCommand, ValidationResult>,
        IRequestHandler<EditProductTopicPageSortCommand, ValidationResult>,
        IRequestHandler<ProductTopicPageSortCommand, ValidationResult>
    {
        private readonly IProductTopicPageRepository _repository;

        public ProductTopicPageCommandHandler(IProductTopicPageRepository repository)
        {
            _repository = repository;
        }
        public void Dispose()
        {
            _repository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductTopicPageAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_repository)) return request.ValidationResult;
            var item = new ProductTopicPage
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Title = request.Title,
                Slug = request.Slug,
                Description = request.Description,
                Keywords = request.Keywords,
                Image = request.Image,
                Icon = request.Icon,
                Icon2 = request.Icon, 
                Tags = request.Tags, 
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName
            };

        //add domain event
        //item.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Add(item);
            return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ProductTopicPageDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository)) return request.ValidationResult;
        var item = new ProductTopicPage
        {
            Id = request.Id
        };

        //add domain event
        //item.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

        public async Task<ValidationResult> Handle(EditProductTopicPageSortCommand request, CancellationToken cancellationToken)
        {
            var datas = await _repository.GetAll();

            List<ProductTopicPage> dataUpdate = new List<ProductTopicPage>();

            foreach (var list in request.List)
            {
                var data = datas.FirstOrDefault(x => x.Id == list.Id);

                if (data is not null)
                {
                    data.DisplayOrder = list.SortOrder ?? 0;
                    dataUpdate.Add(data);
                }
            }
            _repository.Update(dataUpdate);
            return await Commit(_repository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTopicPageEditCommand request, CancellationToken cancellationToken)
    {
            if (!request.IsValid(_repository)) return request.ValidationResult;
            var item = await _repository.GetById(request.Id);

            item.Code = request.Code;
            item.Name = request.Name; 
            item.Title = request.Title;
            item.Slug = request.Slug;
            item.Description = request.Description;
            item.Keywords = request.Keywords;
            item.Image = request.Image;
            item.Icon = request.Icon;
            item.Icon2 = request.Icon2; 
            item.Tags = request.Tags; 
            item.Status = request.Status;
            item.DisplayOrder = request.DisplayOrder; 
            item.UpdatedBy = request.UpdatedBy;
            item.UpdatedDate = request.UpdatedDate;
            item.UpdatedByName = request.UpdatedByName;

            //add domain event
            //item.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _repository.Update(item);
            return await Commit(_repository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTopicPageSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAll();

            List<ProductTopicPage> list = new List<ProductTopicPage>();

            foreach (var sort in request.SortList)
            {
                ProductTopicPage obj = data.FirstOrDefault(c => c.Id == sort.Id);
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

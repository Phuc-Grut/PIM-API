using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductTopicCommandHandler : CommandHandler,
        IRequestHandler<ProductTopicAddCommand, ValidationResult>,
        IRequestHandler<ProductTopicDeleteCommand, ValidationResult>,
        IRequestHandler<ProductTopicEditCommand, ValidationResult>,
        IRequestHandler<EditProductTopicSortCommand, ValidationResult>,
        IRequestHandler<ProductTopicSortCommand, ValidationResult>
    {
        private readonly IProductTopicRepository _repository;
        private readonly IProductTopicPageMapRepository _productTopicPageMapRepository;

        public ProductTopicCommandHandler(IProductTopicRepository repository, IProductTopicPageMapRepository productTopicPageMapRepository)
        {
            _repository = repository;
            _productTopicPageMapRepository = productTopicPageMapRepository;
        }
        public void Dispose()
        {
            _repository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductTopicAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_repository)) return request.ValidationResult;
            var item = new ProductTopic
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Image = request.Image,
                Icon = request.Icon,
                Icon2 = request.Icon, 
                Tags = request.Tags, 
                Status = request.Status,
                Keywords= request.Keywords,
                Slug = request.Slug,
                Title= request.Title,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName
            };

            if (request.ProductTopicPageIds is not null && 
                request.ProductTopicPageCodes is not null && 
                request.ProductTopicPageIds.Any() && 
                request.ProductTopicPageCodes.Any())
            {
                var listAdd = request.ProductTopicPageIds.Select((x, index) => new ProductTopicPageMap
                {
                    Id = Guid.NewGuid(),
                    ProductTopicPageId = x,
                    ProductTopicId = request.Id,
                    ProductTopicPage = request.ProductTopicPageCodes[index],
                    ProductTopic = request.Code,
                    DisplayOrder = request.DisplayOrder,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = request.CreatedDate,
                    CreatedByName = request.CreatedByName
                });

                _productTopicPageMapRepository.Add(listAdd);
            }

            _repository.Add(item);

            return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ProductTopicDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository)) return request.ValidationResult;
        var item = new ProductTopic
        {
            Id = request.Id
        };

        //add domain event
        //item.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

        public async Task<ValidationResult> Handle(EditProductTopicSortCommand request, CancellationToken cancellationToken)
        {
            var datas = await _repository.GetAll();

            List<ProductTopic> dataUpdate = new List<ProductTopic>();

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

        public async Task<ValidationResult> Handle(ProductTopicEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_repository)) return request.ValidationResult;
            var item = await _repository.GetById(request.Id);

            item.Code = request.Code;
            item.Name = request.Name; 
            item.Description = request.Description;
            item.Image = request.Image;
            item.Icon = request.Icon;
            item.Icon2 = request.Icon2; 
            item.Tags = request.Tags; 
            item.Status = request.Status;
            item.Title = request.Title;
            item.Keywords = request.Keywords;
            item.Slug = request.Slug;
            item.DisplayOrder = request.DisplayOrder; 
            item.UpdatedBy = request.UpdatedBy;
            item.UpdatedDate = request.UpdatedDate;
            item.UpdatedByName = request.UpdatedByName;

            if (request.ProductTopicPageIds is not null &&
                request.ProductTopicPageCodes is not null &&
                request.ProductTopicPageIds.Any() &&
                request.ProductTopicPageCodes.Any())
            {
                int index = 0;
                var listAdd = new List<ProductTopicPageMap>();
                foreach (var productTopicPageId in request.ProductTopicPageIds)
                {
                    var dataMapByTopic_TopicPage = await _productTopicPageMapRepository.Filter(request.Id, productTopicPageId);

                    if (dataMapByTopic_TopicPage.Count() == 0) 
                    {
                        var itemAdd = new ProductTopicPageMap
                        {
                            Id = Guid.NewGuid(),
                            ProductTopicPageId = productTopicPageId,
                            ProductTopicId = request.Id,
                            ProductTopicPage = request.ProductTopicPageCodes[index],
                            ProductTopic = request.Code,
                            DisplayOrder = request.DisplayOrder,
                            CreatedBy = request.UpdatedBy,
                            CreatedDate = (DateTime)request.UpdatedDate,
                            CreatedByName = request.UpdatedByName
                        };
                        listAdd.Add(itemAdd);
                    }
                    index++;
                }
                
                _productTopicPageMapRepository.Add(listAdd);

                var dataMapByTopic = await _productTopicPageMapRepository.Filter(request.Id);
                var listRemove = dataMapByTopic.Where(x => !request.ProductTopicPageIds.Contains(x.ProductTopicPageId));

                if (listRemove.Count() > 0)
                {
                    _productTopicPageMapRepository.Remove(listRemove);
                }
            }

            _repository.Update(item);
            return await Commit(_repository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTopicSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAll();

            List<ProductTopic> list = new List<ProductTopic>();

            foreach (var sort in request.SortList)
            {
                ProductTopic obj = data.FirstOrDefault(c => c.Id == sort.Id);
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

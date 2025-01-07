using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductTopicDetailCommandHandler : CommandHandler, IRequestHandler<ProductTopicDetailAddCommand, ValidationResult>,
                                                            IRequestHandler<ProductTopicDetailDeleteCommand, ValidationResult>,
                                                            IRequestHandler<ProductTopicDetailEditCommand, ValidationResult>
    {
        private readonly IProductTopicDetailRepository _itemRepository;

        public ProductTopicDetailCommandHandler(IProductTopicDetailRepository ProductTopicDetailRepository)
        {
            _itemRepository = ProductTopicDetailRepository;
        }
        public void Dispose()
        {
            _itemRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductTopicDetailAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_itemRepository)) return request.ValidationResult;
            var item = new ProductTopicDetail
            {
                Id = request.Id,
                ProductTopic = request.ProductTopic,
                Code = request.Code,
                Condition = request.Condition,
                Unit = request.Unit,
                Name = request.Name,
                SourceLink = request.SourceLink,
                SourceCode = request.SourceCode,
                ShortDescription = request.ShortDescription,
                FullDescription = request.FullDescription,
                Origin = request.Origin,
                Brand = request.Brand,
                Manufacturer = request.Manufacturer,
                Image = request.Image,
                Images = request.Images,
                Price = request.Price,
                Currency = request.Currency,
                Status = request.Status,
                Tags = request.Tags,
                Exp = request.Exp,
                BidPrice = request.BidPrice,
                Tax = request.Tax,
                CreatedBy = request.CreatedBy,
                CreatedByName = request.CreatedByName,
                CreatedDate = request.CreatedDate 
            };

            //add domain event
            //item.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _itemRepository.Add(item);
            return await Commit(_itemRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTopicDetailDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_itemRepository)) return request.ValidationResult;
            var item = new ProductTopicDetail
            {
                Id = request.Id
            };

            //add domain event
            //item.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _itemRepository.Remove(item);
            return await Commit(_itemRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTopicDetailEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_itemRepository)) return request.ValidationResult;
            var data = await _itemRepository.GetById(request.Id);

            data.ProductTopic = request.ProductTopic;
            data.Code = request.Code;
            data.Condition = request.Condition;
            data.Unit = request.Unit;
            data.Name = request.Name;
            data.SourceLink = request.SourceLink;
            data.SourceCode = request.SourceCode;
            data.ShortDescription = request.ShortDescription;
            data.FullDescription = request.FullDescription;
            data.Origin = request.Origin;
            data.Brand = request.Brand;
            data.Manufacturer = request.Manufacturer;
            data.Image = request.Image;
            data.Images = request.Images;
            data.Price = request.Price;
            data.Currency = request.Currency;
            data.Status = request.Status;
            data.Tags = request.Tags;
            data.Exp = request.Exp;
            data.BidPrice = request.BidPrice;
            data.Tax = request.Tax;
            data.UpdatedBy = request.UpdatedBy;
            data.UpdatedDate = request.UpdatedDate;
            data.UpdatedByName = request.UpdatedByName;


            //add domain event
            //item.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _itemRepository.Update(data);
            return await Commit(_itemRepository.UnitOfWork);
        }

         
    }
}

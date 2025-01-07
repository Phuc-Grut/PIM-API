using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ServiceAddCommandHandler : CommandHandler, IRequestHandler<ServiceAddAddCommand, ValidationResult>,
                                                        IRequestHandler<ServiceAddDeleteCommand, ValidationResult>,
                                                        IRequestHandler<ServiceAddEditCommand, ValidationResult>,
                                                        IRequestHandler<ServiceAddSortCommand, ValidationResult>
    {
        private readonly IServiceAddRepository _serviceAddRepository;
        private readonly IProductServiceAddRepository _productServiceAddRepository;

        public ServiceAddCommandHandler(IServiceAddRepository ServiceAddRepository, IProductServiceAddRepository productServiceAddRepository)
        {
            _serviceAddRepository = ServiceAddRepository;
            _productServiceAddRepository = productServiceAddRepository;
        }
        public void Dispose()
        {
            _serviceAddRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ServiceAddAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_serviceAddRepository)) return request.ValidationResult;
            var serviceAdd = new ServiceAdd
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                CalculationMethod = request.CalculationMethod,
                Price = request.Price,
                PriceSyntax = request.PriceSyntax,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                Currency = request.Currency,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName

            };

            //add domain event
            //serviceAdd.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _serviceAddRepository.Add(serviceAdd);
            return await Commit(_serviceAddRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ServiceAddDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_serviceAddRepository)) return request.ValidationResult;

            var filter = new Dictionary<string, object> { { "serviceAddId", request.Id } };

            var productServiceAdds = await _productServiceAddRepository.Filter(filter);
            if (productServiceAdds.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var serviceAdd = new ServiceAdd
            {
                Id = request.Id
            };

            _serviceAddRepository.Remove(serviceAdd);
            return await Commit(_serviceAddRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ServiceAddEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_serviceAddRepository)) return request.ValidationResult;
            var data = await _serviceAddRepository.GetById(request.Id);
            var serviceAdd = new ServiceAdd
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                CalculationMethod = request.CalculationMethod,
                Price = request.Price,
                PriceSyntax = request.PriceSyntax,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                Currency = request.Currency,
                CreatedBy = data.CreatedBy,
                CreatedDate = data.CreatedDate,
                CreatedByName = data.CreatedByName,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate,
                UpdatedByName = request.UpdatedByName
            };

            //add domain event
            //serviceAdd.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _serviceAddRepository.Update(serviceAdd);
            return await Commit(_serviceAddRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ServiceAddSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _serviceAddRepository.GetAll();

            List<ServiceAdd> serviceAdds = new List<ServiceAdd>();

            foreach (var sort in request.SortList)
            {
                ServiceAdd serviceAdd = data.FirstOrDefault(c => c.Id == sort.Id);
                if (serviceAdd != null)
                {
                    serviceAdd.DisplayOrder = sort.SortOrder;
                    serviceAdds.Add(serviceAdd);
                }
            }
            _serviceAddRepository.Update(serviceAdds);
            return await Commit(_serviceAddRepository.UnitOfWork);
        }
    }
}

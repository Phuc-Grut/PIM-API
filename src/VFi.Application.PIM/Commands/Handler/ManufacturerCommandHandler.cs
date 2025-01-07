using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands.Handler
{
    internal class ManufacturerCommandHandler : CommandHandler, IRequestHandler<ManufacturerAddCommand, ValidationResult>,
                                                                IRequestHandler<ManufacturerDeleteCommand, ValidationResult>,
                                                                IRequestHandler<ManufacturerEditCommand, ValidationResult>,
                                                                IRequestHandler<ManufacturerSortCommand, ValidationResult>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IProductRepository _productRepository;

        public ManufacturerCommandHandler(IManufacturerRepository ManufacturerRepository, IProductRepository productRepository)
        {
            _manufacturerRepository = ManufacturerRepository;
            _productRepository = productRepository;
        }
        public void Dispose()
        {
            _manufacturerRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ManufacturerAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_manufacturerRepository)) return request.ValidationResult;
            var manufacturer = new Manufacturer
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName,
            };

            //add domain event
            //manufacturer.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _manufacturerRepository.Add(manufacturer);
            return await Commit(_manufacturerRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ManufacturerDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_manufacturerRepository)) return request.ValidationResult;

            var filter = new Dictionary<string, object> { { "manufacturerId", request.Id } };

            var products = await _productRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var manufacturer = new Manufacturer
            {
                Id = request.Id
            };

            _manufacturerRepository.Remove(manufacturer);
            return await Commit(_manufacturerRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ManufacturerEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_manufacturerRepository)) return request.ValidationResult;
            var data = await _manufacturerRepository.GetById(request.Id);
            var manufacturer = new Manufacturer
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
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
            //manufacturer.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _manufacturerRepository.Update(manufacturer);
            return await Commit(_manufacturerRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ManufacturerSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _manufacturerRepository.GetAll();

            List<Manufacturer> manufacturers = new List<Manufacturer>();

            foreach (var sort in request.SortList)
            {
                Manufacturer manufacturer = data.FirstOrDefault(c => c.Id == sort.Id);
                if (manufacturer != null)
                {
                    manufacturer.DisplayOrder = sort.SortOrder;
                    manufacturers.Add(manufacturer);
                }
            }
            _manufacturerRepository.Update(manufacturers);
            return await Commit(_manufacturerRepository.UnitOfWork);
        }
    }
}

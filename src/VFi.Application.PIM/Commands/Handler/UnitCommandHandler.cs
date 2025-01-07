using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class UnitCommandHandler : CommandHandler, IRequestHandler<UnitAddCommand, ValidationResult>,
                                                        IRequestHandler<UnitDeleteCommand, ValidationResult>,
                                                        IRequestHandler<UnitEditCommand, ValidationResult>,
                                                        IRequestHandler<UnitSortCommand, ValidationResult>
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IProductRepository _productRepository;

        public UnitCommandHandler(IUnitRepository UnitRepository, IProductRepository productRepository)
        {
            _unitRepository = UnitRepository;
            _productRepository = productRepository;
        }
        public void Dispose()
        {
            _unitRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(UnitAddCommand cmd, CancellationToken cancellationToken)
        {
            if (!cmd.IsValid(_unitRepository)) return cmd.ValidationResult;
            var unit = new Domain.PIM.Models.Unit
            {
                Id = cmd.Id,
                Code = cmd.Code,
                Name = cmd.Name,
                Rate = cmd.Rate,
                IsDefault = cmd.IsDefault,
                DisplayLocale = cmd.DisplayLocale,
                GroupUnitId = cmd.GroupUnitId,
                NamePlural = cmd.NamePlural,
                Description = cmd.Description,
                Status = cmd.Status,
                DisplayOrder = cmd.DisplayOrder,
                CreatedBy = cmd.CreatedBy,
                CreatedDate = cmd.CreatedDate,
                CreatedByName = cmd.CreatedByName
            };

            //add domain event
            //unit.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _unitRepository.Add(unit);
            return await Commit(_unitRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UnitDeleteCommand cmd, CancellationToken cancellationToken)
        {
            if (!cmd.IsValid(_unitRepository)) return cmd.ValidationResult;

            var filter = new Dictionary<string, object> { { "unitId", cmd.Id } };

            var products = await _productRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var unit = new Domain.PIM.Models.Unit
            {
                Id = cmd.Id
            };

            _unitRepository.Remove(unit);
            return await Commit(_unitRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UnitEditCommand cmd, CancellationToken cancellationToken)
        {
            if (!cmd.IsValid(_unitRepository)) return cmd.ValidationResult;
            var unit = await _unitRepository.GetById(cmd.Id);

            unit.Code = cmd.Code;
            unit.Name = cmd.Name;
            unit.Rate = cmd.Rate;
            unit.IsDefault = cmd.IsDefault;
            unit.DisplayLocale = cmd.DisplayLocale;
            unit.GroupUnitId = cmd.GroupUnitId;
            unit.NamePlural = cmd.NamePlural;
            unit.Description = cmd.Description;
            unit.Status = cmd.Status;
            unit.DisplayOrder = cmd.DisplayOrder;
            unit.UpdatedBy = cmd.UpdatedBy;
            unit.UpdatedDate = cmd.UpdatedDate;
            unit.UpdatedByName = cmd.UpdatedByName;
          

            //add domain event
            //unit.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _unitRepository.Update(unit);
            return await Commit(_unitRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UnitSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitRepository.GetAll();

            List<Domain.PIM.Models.Unit> list = new List<Domain.PIM.Models.Unit>();

            foreach (var sort in request.SortList)
            {
                Domain.PIM.Models.Unit obj = data.FirstOrDefault(c => c.Id == sort.Id);
                if (obj != null)
                {
                    obj.DisplayOrder = sort.SortOrder;
                    list.Add(obj);
                }
            }
            _unitRepository.Update(list);
            return await Commit(_unitRepository.UnitOfWork);
        }
    }
}

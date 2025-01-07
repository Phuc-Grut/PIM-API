using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class GroupUnitCommandHandler : CommandHandler, IRequestHandler<GroupUnitAddCommand, ValidationResult>,
                                                            IRequestHandler<GroupUnitDeleteCommand, ValidationResult>,
                                                            IRequestHandler<GroupUnitEditCommand, ValidationResult>,
                                                            IRequestHandler<GroupUnitSortCommand, ValidationResult>
    {
        private readonly IGroupUnitRepository _groupUnitRepository;

        public GroupUnitCommandHandler(IGroupUnitRepository GroupUnitRepository)
        {
            _groupUnitRepository = GroupUnitRepository;
        }
        public void Dispose()
        {
            _groupUnitRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(GroupUnitAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_groupUnitRepository)) return request.ValidationResult;
            var groupUnit = new GroupUnit
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

            //add domain event
            //groupUnit.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _groupUnitRepository.Add(groupUnit);
            return await Commit(_groupUnitRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(GroupUnitDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_groupUnitRepository)) return request.ValidationResult;
            var groupUnit = new GroupUnit
            {
                Id = request.Id
            };

            //add domain event
            //groupUnit.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _groupUnitRepository.Remove(groupUnit);
            return await Commit(_groupUnitRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(GroupUnitEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_groupUnitRepository)) return request.ValidationResult;
            var data = await _groupUnitRepository.GetById(request.Id);
            var groupUnit = new GroupUnit
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
            //groupUnit.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _groupUnitRepository.Update(groupUnit);
            return await Commit(_groupUnitRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(GroupUnitSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _groupUnitRepository.GetAll();

            List<GroupUnit> list = new List<GroupUnit>();

            foreach (var sort in request.SortList)
            {
                GroupUnit obj = data.FirstOrDefault(c => c.Id == sort.Id);
                if (obj != null)
                {
                    obj.DisplayOrder = sort.SortOrder;
                    list.Add(obj);
                }
            }
            _groupUnitRepository.Update(list);
            return await Commit(_groupUnitRepository.UnitOfWork);
        }
    }
}

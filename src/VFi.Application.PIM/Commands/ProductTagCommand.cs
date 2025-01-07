using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands
{
    public class ProductTagCommand : Command
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int? Type { get; set; }
    }

    public class ProductTagAddCommand : ProductTagCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductTagAddCommand(
            Guid id,
            string name,
            int status,
            int? type,
            Guid createdBy,
            DateTime createdDate)
        {
            Id = id;
            Name = name;
            Status = status;
            Type = type;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductTagAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductTagEditCommand : ProductTagCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ProductTagEditCommand(
           Guid id,
            string name,
            int status,
            int? type,
            Guid? updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            Name = name;
            Status = status;
            Type = type;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }
        public bool IsValid(IProductTagRepository _context)
        {
            ValidationResult = new ProductTagEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductTagDeleteCommand : ProductTagCommand
    {
        public ProductTagDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductTagRepository _context)
        {
            ValidationResult = new ProductTagDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
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
    public class ProductMediaCommand : Command
    {

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string Path { get; set; } = null!;
        public string MediaType { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public List<ProductMediaDto>? ListAtt { get; set; }
    }

    public class ProductMediaAddCommand : ProductMediaCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductMediaAddCommand(
            Guid id,
            Guid productId,
            string? name,
            string path,
            string mediaType,
            int displayOrder,
            Guid createdBy,
            DateTime createdDate)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            Path = path;
            MediaType = mediaType;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductMediaAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ProductMediaAddListCommand : ProductMediaCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductMediaAddListCommand(
          List<ProductMediaDto>? listAtt
           )
        {
            ListAtt = listAtt;
        }
    }
    public class ProductMediaEditCommand : ProductMediaCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ProductMediaEditCommand(
            Guid id,
            Guid productId,
            string? name,
            string path,
            string mediaType,
            int displayOrder,
            Guid? updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            Path = path;
            MediaType = mediaType;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductMediaEditCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductMediaDeleteCommand : ProductMediaCommand
    {
        public ProductMediaDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductMediaRepository _context)
        {
            ValidationResult = new ProductMediaDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

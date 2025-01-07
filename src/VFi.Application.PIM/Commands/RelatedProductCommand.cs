using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation;
using MassTransit.Internals.GraphValidation;
using Microsoft.AspNetCore.Server.IISIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VFi.Application.PIM.Commands
{
    public class RelatedProductCommand : Command
    {

        public Guid Id { get; set; }
        public Guid ProductId1 { get; set; }
        public Guid ProductId2 { get; set; }
        public int DisplayOrder { get; set; }
    
    }

    public class RelatedProductAddCommand : RelatedProductCommand
    {
        public List<Guid> ListProductId2 { get; set; }
        public RelatedProductAddCommand(
              Guid productId1,
           List<Guid> listProductId2
          )
        {
            ProductId1 = productId1;
            ListProductId2 = listProductId2;
        }
    }

    public class RelatedProductEditCommand : RelatedProductCommand
    {
        public RelatedProductEditCommand(
                Guid id,
            Guid productId1,
            Guid productId2,
            int displayOrder)
        {
            Id = id;
            ProductId1 = productId1;
            ProductId2 = productId2;
            DisplayOrder = displayOrder;
        }
        public bool IsValid(IRelatedProductRepository _context)
        {
            ValidationResult = new RelatedProductEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
      
    }

    public class RelatedProductDeleteCommand : RelatedProductCommand
    {
        public RelatedProductDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IRelatedProductRepository _context)
        {
            ValidationResult = new RelatedProductDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

using Consul;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands
{
     
    public class ProductAddFromLinkCommand : Command
    {
        public ProductAddFromLinkCommand(Guid id, string code, string link, int status, Guid createdBy, string createdByName)
        {
            Id = id;
            Code = code;
            Link = link;
            Status = status;
            CreatedBy = createdBy;
            CreatedByName = createdByName;
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
        public int Status { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; }
         
        public bool IsValid(IProductRepository _context)

        {
            ValidationResult = new ProductAddFromLinkCommandValidation(_context,Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    
     
}

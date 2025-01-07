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
    public class TierPriceCommand : Command
    {

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid? StoreId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CalculationMethod { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class TierPriceAddCommand : TierPriceCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public TierPriceAddCommand(
            Guid id,
            Guid? storeId,
            Guid productId,
            DateTime? startDate,
            DateTime? endDate,
            decimal price,  
            int calculationMethod,
            int quantity,
            Guid createdBy,
            DateTime createdDate)
        {
            Id = id;
            ProductId = productId;
            StoreId= storeId;
            StartDate = startDate;
            EndDate = endDate;
            Price = price;
            CalculationMethod = calculationMethod;
            Quantity = quantity;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
        }
        public bool IsValid()
        {
            ValidationResult = new TierPriceAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class TierPriceEditCommand : TierPriceCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public TierPriceEditCommand(
            Guid id,
            Guid? storeId,
            Guid productId,
            DateTime? startDate,
            DateTime? endDate,
            decimal price,
            int calculationMethod,
            int quantity,
            Guid? updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            ProductId = productId;
            StoreId = storeId;
            StartDate = startDate;
            EndDate = endDate;
            Price = price;
            CalculationMethod = calculationMethod;
            Quantity = quantity;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }
        public bool IsValid(ITierPriceRepository _context)
        {
            ValidationResult = new TierPriceEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
      
    }

    public class TierPriceDeleteCommand : TierPriceCommand
    {
        public TierPriceDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(ITierPriceRepository _context)
        {
            ValidationResult = new TierPriceDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

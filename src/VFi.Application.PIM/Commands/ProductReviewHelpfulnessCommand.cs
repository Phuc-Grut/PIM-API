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
    public class ProductReviewHelpfulnessCommand : Command
    {

        public Guid Id { get; set; }
        public Guid ProductReviewId { get; set; }
        public bool WasHelpful { get; set; }
    }

    public class ProductReviewHelpfulnessAddCommand : ProductReviewHelpfulnessCommand
    {
        public ProductReviewHelpfulnessAddCommand(
            Guid id,
            Guid productReviewId,
            bool wasHelpful
            )
        {
            Id = id;
            ProductReviewId= productReviewId;
            WasHelpful= wasHelpful;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductReviewHelpfulnessAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductReviewHelpfulnessEditCommand : ProductReviewHelpfulnessCommand
    {
        public ProductReviewHelpfulnessEditCommand(
            Guid id,
            Guid productReviewId,
            bool wasHelpful
            )
        {
            Id = id;
            ProductReviewId = productReviewId;
            WasHelpful = wasHelpful;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductReviewHelpfulnessEditCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductReviewHelpfulnessDeleteCommand : ProductReviewHelpfulnessCommand
    {
        public ProductReviewHelpfulnessDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductReviewHelpfulnessRepository _context)
        {
            ValidationResult = new ProductReviewHelpfulnessDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

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
    public class ProductReviewCommand : Command
    {

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Title { get; set; }
        public string? ReviewText { get; set; }
        public int Rating { get; set; }
        public int HelpfulYesTotal { get; set; }
        public int HelpfulNoTotal { get; set; }
        public bool? IsVerifiedPurchase { get; set; }
    }

    public class ProductReviewAddCommand : ProductReviewCommand
    {
        public ProductReviewAddCommand(
            Guid id,
            Guid productId,
            string? title,
            string? reviewText,
            int rating,
            int helpfulYesTotal,
            int helpfulNoTotal,
            bool? isVerifiedPurchase)
        {
            Id = id;
            ProductId = productId;
            Title = title;
            ReviewText = reviewText;
            Rating = rating;
            HelpfulYesTotal = helpfulYesTotal;
            HelpfulNoTotal = helpfulNoTotal;
            IsVerifiedPurchase = isVerifiedPurchase;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductReviewAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductReviewEditCommand : ProductReviewCommand
    {
        public ProductReviewEditCommand(
            Guid id,
            Guid productId,
            string? title,
            string? reviewText,
            int rating,
            int helpfulYesTotal,
            int helpfulNoTotal,
            bool? isVerifiedPurchase)
        {
            Id = id;
            ProductId = productId;
            Title = title;
            ReviewText = reviewText;
            Rating = rating;
            HelpfulYesTotal = helpfulYesTotal;
            HelpfulNoTotal = helpfulNoTotal;
            IsVerifiedPurchase = isVerifiedPurchase;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductReviewEditCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductReviewDeleteCommand : ProductReviewCommand
    {
        public ProductReviewDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductReviewRepository _context)
        {
            ValidationResult = new ProductReviewDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

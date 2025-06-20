﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.PIM.Models
{
    public partial class ProductReview : Entity, IAggregateRoot
    {
        public ProductReview()
        {
            ProductReviewHelpfulness = new HashSet<ProductReviewHelpfulness>();
        }
         
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public int HelpfulYesTotal { get; set; }
        public int HelpfulNoTotal { get; set; }
        public bool? IsVerifiedPurchase { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<ProductReviewHelpfulness> ProductReviewHelpfulness { get; set; }
    }
}
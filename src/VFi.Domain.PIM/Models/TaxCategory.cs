﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.PIM.Models
{
    public partial class TaxCategory : Entity, IAggregateRoot
    {
        public TaxCategory()
        {
            Product = new HashSet<Product>();
        }
         
        public string Code { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public int? Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
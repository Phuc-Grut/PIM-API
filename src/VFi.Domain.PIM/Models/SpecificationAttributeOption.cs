﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.PIM.Models
{
    public partial class SpecificationAttributeOption : Entity, IAggregateRoot
    {
        public SpecificationAttributeOption()
        {
            ProductSpecificationAttributeMapping = new HashSet<ProductSpecificationAttributeMapping>();
        }
         
        public Guid SpecificationAttributeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal NumberValue { get; set; }
        public string Color { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public string CreatedByName { get; set; }

        public virtual SpecificationAttribute SpecificationAttribute { get; set; }
        public virtual ICollection<ProductSpecificationAttributeMapping> ProductSpecificationAttributeMapping { get; set; }
    }
}
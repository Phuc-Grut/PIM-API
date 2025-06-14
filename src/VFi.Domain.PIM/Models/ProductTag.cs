﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.PIM.Models
{
    public partial class ProductTag : Entity, IAggregateRoot
    {
        public ProductTag()
        {
            ProductProductTagMapping = new HashSet<ProductProductTagMapping>();
        }
         
        public string Name { get; set; }
        public int? Type { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public string CreatedByName { get; set; }

        public virtual ICollection<ProductProductTagMapping> ProductProductTagMapping { get; set; }
    }
}
﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.PIM.Models
{
    public partial class ProductPackage : Entity, IAggregateRoot
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public string CreatedByName { get; set; }

        public virtual Product Product { get; set; }
    }
}
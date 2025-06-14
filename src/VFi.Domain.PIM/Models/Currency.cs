﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;

namespace VFi.Domain.PIM.Models
{
    public partial class Currency : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public string DisplayLocale { get; set; }
        public string CustomFormatting { get; set; }
        public int DisplayOrder { get; set; }
        public bool RoundOrderItemsEnabled { get; set; }
        public int RoundNumDecimals { get; set; }
        public bool RoundOrderTotalEnabled { get; set; }
        public decimal RoundOrderTotalDenominator { get; set; }
        public int RoundOrderTotalRule { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public string CreatedByName { get; set; }
    }
}
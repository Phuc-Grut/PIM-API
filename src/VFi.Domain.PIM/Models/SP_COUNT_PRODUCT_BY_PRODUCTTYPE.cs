﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VFi.Domain.PIM.Models
{
    public partial class SP_COUNT_PRODUCT_BY_PRODUCTTYPE
    {
        public Guid Id { get; set; }
        public string? ProductType { get; set; }
        public int? TotalCountByType { get; set; }
    }
}

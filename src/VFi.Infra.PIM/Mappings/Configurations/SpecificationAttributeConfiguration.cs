﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.PIM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.PIM.Mappings.Configurations
{
    public partial class SpecificationAttributeConfiguration : IEntityTypeConfiguration<SpecificationAttribute>
    {
        public void Configure(EntityTypeBuilder<SpecificationAttribute> entity)
        {
            entity.ToTable("SpecificationAttribute", "pim");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Alias).HasMaxLength(30);

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SpecificationAttribute> entity);
    }
}

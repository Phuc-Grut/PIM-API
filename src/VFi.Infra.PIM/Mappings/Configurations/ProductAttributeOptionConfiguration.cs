﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.PIM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.PIM.Mappings.Configurations
{
    public partial class ProductAttributeOptionConfiguration : IEntityTypeConfiguration<ProductAttributeOption>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeOption> entity)
        {
            entity.ToTable("ProductAttributeOption", "pim");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Alias).HasMaxLength(100);

            entity.Property(e => e.Color).HasMaxLength(100);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Image).HasMaxLength(255);

            entity.Property(e => e.Name).HasMaxLength(4000);

            entity.Property(e => e.PriceAdjustment).HasColumnType("decimal(18, 4)");

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.Property(e => e.WeightAdjustment).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.ProductAttribute)
                .WithMany(p => p.ProductAttributeOption)
                .HasForeignKey(d => d.ProductAttributeId)
                .HasConstraintName("FK_ProductAttributeOption_ProductAttribute");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ProductAttributeOption> entity);
    }
}

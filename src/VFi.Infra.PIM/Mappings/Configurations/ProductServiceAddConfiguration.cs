﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.PIM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.PIM.Mappings.Configurations
{
    public partial class ProductServiceAddConfiguration : IEntityTypeConfiguration<ProductServiceAdd>
    {
        public void Configure(EntityTypeBuilder<ProductServiceAdd> entity)
        {
            entity.ToTable("ProductServiceAdd", "pim");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.MaxPrice).HasColumnType("money");

            entity.Property(e => e.MinPrice).HasColumnType("money");

            entity.Property(e => e.PayRequired).HasDefaultValueSql("((0))");

            entity.Property(e => e.Price).HasColumnType("money");

            entity.Property(e => e.PriceSyntax).HasMaxLength(500);

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductServiceAdd)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductServiceAdd_Product");

            entity.HasOne(d => d.ServiceAdd)
                .WithMany(p => p.ProductServiceAdd)
                .HasForeignKey(d => d.ServiceAddId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductServiceAdd_ServiceAdd");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ProductServiceAdd> entity);
    }
}

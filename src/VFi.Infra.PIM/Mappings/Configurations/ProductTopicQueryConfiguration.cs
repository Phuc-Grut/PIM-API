﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.PIM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.PIM.Mappings.Configurations
{
    public partial class ProductTopicQueryConfiguration : IEntityTypeConfiguration<ProductTopicQuery>
    {
        public void Configure(EntityTypeBuilder<ProductTopicQuery> entity)
        {
            entity.ToTable("ProductTopicQuery", "pim");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.BrandId).HasMaxLength(250);

            entity.Property(e => e.Category).HasMaxLength(250);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Keyword).HasMaxLength(250);

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.Property(e => e.Seller).HasMaxLength(250);

            entity.Property(e => e.SourceCode).HasMaxLength(50);

            entity.Property(e => e.SourcePath).HasMaxLength(250);

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.ProductTopic)
                .WithMany(p => p.ProductTopicQuery)
                .HasForeignKey(d => d.ProductTopicId)
                .HasConstraintName("FK_ProductTopicQuery_ProductTopic");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ProductTopicQuery> entity);
    }
}

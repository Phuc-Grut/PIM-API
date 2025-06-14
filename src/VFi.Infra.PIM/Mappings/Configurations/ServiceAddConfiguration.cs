﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.PIM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.PIM.Mappings.Configurations
{
    public partial class ServiceAddConfiguration : IEntityTypeConfiguration<ServiceAdd>
    {
        public void Configure(EntityTypeBuilder<ServiceAdd> entity)
        {
            entity.ToTable("ServiceAdd", "pim");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.MaxPrice).HasColumnType("money");

            entity.Property(e => e.MinPrice).HasColumnType("money");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(400);

            entity.Property(e => e.Price).HasColumnType("money");

            entity.Property(e => e.PriceSyntax).HasMaxLength(500);

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ServiceAdd> entity);
    }
}

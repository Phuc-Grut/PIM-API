﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.PIM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.PIM.Mappings.Configurations
{
    public partial class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> entity)
        {
            entity.ToTable("Manufacturer", "pim");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.Property(e => e.Image).HasMaxLength(255);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(400);

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Manufacturer> entity);
    }
}

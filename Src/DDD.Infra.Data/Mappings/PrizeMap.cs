using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Infra.Data.Mappings
{
    public class PrizeMap : IEntityTypeConfiguration<Prize>
    {
        public void Configure(EntityTypeBuilder<Prize> builder)
        {
            builder.Property(c => c.Id)
               .HasColumnName("Id");

            builder.Property(c => c.Name)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

            builder.Property(c => c.Description)
           .HasColumnType("varchar(100)")
           .HasMaxLength(100)
           .IsRequired();

            builder.Property(c => c.CountryId)
           .HasColumnType("varchar(100)")
           .HasMaxLength(100)
           .IsRequired();

            builder.Property(c => c.WebSiteUrl)
         .HasColumnType("varchar(100)")
         .HasMaxLength(100)
         .IsRequired();


            builder.Property(c => c.FacebookUrl)
         .HasColumnType("varchar(100)")
         .HasMaxLength(100)
         .IsRequired();

            builder.Property(c => c.Logo)
       .HasColumnType("varchar(100)")
       .HasMaxLength(100)
       .IsRequired();


        }
    }
}

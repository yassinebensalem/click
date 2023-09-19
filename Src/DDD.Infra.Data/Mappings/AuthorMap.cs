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
    public class AuthorMap : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(c => c.Id)
               .HasColumnName("Id");

            builder.Property(c => c.FirstName)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

            builder.Property(c => c.LastName)
           .HasColumnType("varchar(100)")
           .HasMaxLength(100)
           .IsRequired();

            builder.Property(c => c.Email)
           .HasColumnType("varchar(100)")
           .HasMaxLength(100)
           .IsRequired();

            builder.Property(c => c.Biography)
         .HasColumnType("varchar(100)")
         .HasMaxLength(100)
         .IsRequired();

            builder.Property(c => c.PhoneNumber)
           .HasColumnType("varchar(100)")
           .HasMaxLength(100)
           .IsRequired();

            builder.Property(c => c.Birthdate)
         .HasColumnType("varchar(100)")
         .HasMaxLength(100)
         .IsRequired();

            builder.Property(c => c.PhotoPath)
         .HasColumnType("varchar(100)")
         .HasMaxLength(100)
         .IsRequired();

            builder.HasQueryFilter(p => !p.IsDeleted);


        }

    }
}

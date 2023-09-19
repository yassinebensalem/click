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
    public class PrizeBookMap  : IEntityTypeConfiguration<PrizeBook>
    {
        public void Configure(EntityTypeBuilder<PrizeBook> builder)
    {
        builder.Property(c => c.Id)
           .HasColumnName("Id");

        builder.Property(c => c.PrizeId)
        .HasColumnName("PrizeId");

        builder.Property(c => c.BookId)
       .HasColumnName("BookId");

        builder.Property(c => c.Edition)
       .HasColumnType("varchar(100)")
       .HasMaxLength(100)
       .IsRequired();


    }
        }
 }


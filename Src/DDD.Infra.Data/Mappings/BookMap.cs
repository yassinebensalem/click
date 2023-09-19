using System.Diagnostics;
using DDD.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Infra.Data.Mappings
{
    public class BookMap : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("Id");

            builder.Property(c => c.Title)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.PageNumbers)
                .HasColumnType("int") 
                .IsRequired();

            builder.Property(c => c.CoverPath)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Price)
                .HasColumnType("float")
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnType("varchar(MAX)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.PublicationDate)
                .HasColumnType("datetime2(7)")
                .IsRequired();

            builder.Property(c => c.ISBN)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.ISSN)
                            .HasColumnType("varchar(100)")
                            .HasMaxLength(100)
                            .IsRequired();

            builder.Property(c => c.EISBN)
                            .HasColumnType("varchar(100)")
                            .HasMaxLength(100)
                            .IsRequired();

            builder.Property(c => c.PDFPath)
                            .HasColumnType("varchar(MAX)")
                            .HasMaxLength(500)
                            .IsRequired();
            //builder.Property(c => c.IsFree);
            // builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            builder.HasQueryFilter(p => !p.IsDeleted);

            // builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            builder.HasQueryFilter(p => !p.IsDeleted);

            builder.HasMany<Invoice>(b => b.Invoices)
            .WithOne(i=>i.Book)
            .HasForeignKey(i => i.BookId);
        }
    }
}

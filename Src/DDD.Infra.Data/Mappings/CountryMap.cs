using DDD.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DDD.Infra.Data.Mappings

{
    public class CountryMap : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("Id");

            builder.Property(c => c.Name)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();



            builder.Property(c => c.CodeAlpha2)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();



            builder.Property(c => c.CodeAlpha3)
                .HasColumnType("varchar(MAX)")
                .HasMaxLength(100)
                .IsRequired();



           
        }
    }
}

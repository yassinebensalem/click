using DDD.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DDD.Infra.Data.Mappings
{
    public class LanguageMap : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
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

          

            builder.Property(c => c.Code)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

           

            // builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            //builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}

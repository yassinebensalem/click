using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Common.JSONColumns;
using DDD.Domain.EventHandlers;
using DDD.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace DDD.Infra.Data.Mappings
{
    public class JsonValueConverter<TEntity> : ValueConverter<TEntity, string>
    {
        public JsonValueConverter(JsonSerializerSettings serializerSettings = null,
                                  ConverterMappingHints mappingHints = null)
            : base(model => JsonConvert.SerializeObject(model, serializerSettings),
                   value => JsonConvert.DeserializeObject<TEntity>(value, serializerSettings),
                   mappingHints)
        {
            //No ctor body; everything is passed through the call to base()
        }

        public static ValueConverter Default { get; } =
            new JsonValueConverter<TEntity>(null, null);

        public static ValueConverterInfo DefaultInfo { get; } =
            new ValueConverterInfo(typeof(TEntity),
                typeof(string),
                i => new JsonValueConverter<TEntity>(null, i.MappingHints));
    }
    public class JoinRequestMap : IEntityTypeConfiguration<JoinRequest>
    {
      
        public void Configure(EntityTypeBuilder<JoinRequest> builder)
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

            builder.Property(c => c.Description)
         .HasColumnType("varchar(100)")
         .HasMaxLength(100)
         .IsRequired();

            builder.Property(c => c.PhoneNumber)
           .HasColumnType("varchar(100)")
           .HasMaxLength(100)
           .IsRequired();

            builder.Property(c => c.RequesterType)
         .HasColumnType("varchar(100)")
         .HasMaxLength(100)
         .IsRequired();

            builder.Property(c => c.Status)
         .HasColumnType("varchar(100)")
         .HasMaxLength(100)
         .IsRequired();

            builder.Property(c => c.VoucherNumber)
         .HasColumnType("varchar(100)")
         .HasMaxLength(100);

            builder.Property(c => c.VoucherValue)
      .HasColumnType("float");

            builder.Property(m => m.DesiredBooks)
                 .HasColumnType("nvarchar(MAX)");

            builder.Property(c => c.ReceiverEmail)
       .HasColumnType("varchar(100)")
       .HasMaxLength(100);
        }
    }
}

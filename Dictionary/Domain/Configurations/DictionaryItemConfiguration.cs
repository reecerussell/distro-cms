using Dictionary.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entity;

namespace Dictionary.Domain.Configurations
{
    public class DictionaryItemConfiguration : BaseEntityConfiguration<DictionaryItem>
    {
        public override void Configure(EntityTypeBuilder<DictionaryItem> builder)
        {
            builder.ToTable("DictionaryItems");

            builder
                .Property(x => x.Key)
                .HasMaxLength(45)
                .IsRequired();

            builder
                .Property(x => x.CultureName)
                .HasMaxLength(14)
                .IsRequired();

            builder
                .HasIndex(x => new {x.Key, x.CultureName});

            builder
                .Property(x => x.DisplayName)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(x => x.Value)
                .HasMaxLength(255)
                .IsRequired();

            base.Configure(builder);
        }
    }
}

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
                .Property(x => x.CultureId)
                .HasMaxLength(36)
                .IsRequired();

            builder
                .HasIndex(x => new {x.Key, x.CultureId});

            builder
                .Property(x => x.DisplayName)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(x => x.Value)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .HasOne(x => x.Culture)
                .WithMany(x => x.DictionaryItems)
                .HasForeignKey(x => x.CultureId);

            base.Configure(builder);
        }
    }
}

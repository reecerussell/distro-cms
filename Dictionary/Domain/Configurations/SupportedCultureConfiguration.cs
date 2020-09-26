using Dictionary.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entity;

namespace Dictionary.Domain.Configurations
{
    public class SupportedCultureConfiguration : BaseEntityConfiguration<SupportedCulture>
    {
        public override void Configure(EntityTypeBuilder<SupportedCulture> builder)
        {
            builder.ToTable("SupportedCultures");

            builder.Property(x => x.Name)
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(x => x.DisplayName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.IsDefault)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasIndex(x => x.Name);

            base.Configure(builder);
        }
    }
}

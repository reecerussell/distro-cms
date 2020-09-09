using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entity;

namespace Infrastructure.Configurations
{
    internal class PageTranslationConfiguration : BaseEntityConfiguration<PageTranslation>
    {
        public override void Configure(EntityTypeBuilder<PageTranslation> builder)
        {
            builder.ToTable("PageTranslations");

            builder
                .Property(x => x.Title)
                .HasMaxLength(255);

            builder
                .Property(x => x.Description)
                .HasMaxLength(255);

            builder
                .Property(x => x.CultureName)
                .HasMaxLength(5);

            builder
                .Property(x => x.PageId)
                .HasMaxLength(36);

            base.Configure(builder);
        }
    }
}

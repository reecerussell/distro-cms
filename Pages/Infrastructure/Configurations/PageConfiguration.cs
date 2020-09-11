using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pages.Domain.Models;
using Shared.Entity;

namespace Pages.Infrastructure.Configurations
{
    public class PageConfiguration : BaseEntityConfiguration<Page>
    {
        public override void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.ToTable("Pages");

            builder
                .Property(x => x.Url)
                .HasMaxLength(255);

            builder
                .HasMany(x => x.Translations)
                .WithOne()
                .HasForeignKey(x => x.PageId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}

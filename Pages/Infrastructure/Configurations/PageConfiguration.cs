using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entity;

namespace Infrastructure.Configurations
{
    internal class PageConfiguration : BaseEntityConfiguration<Page>
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

using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entity;

namespace Infrastructure.Configurations
{
    internal class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(x => x.Firstname)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(x => x.Lastname)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(x => x.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            base.Configure(builder);
        }
    }
}

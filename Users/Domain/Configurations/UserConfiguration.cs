using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entity;
using Users.Domain.Models;

namespace Users.Domain.Configurations
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

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

            builder
                .HasMany(x => x.Roles)
                .WithOne()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entity;
using Users.Domain.Models;

namespace Users.Domain.Configurations
{
    public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder
                .Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(36);

            builder
                .Property(x => x.RoleId)
                .IsRequired()
                .HasMaxLength(36);

            builder.HasIndex(x => new {x.UserId, x.RoleId});

            builder
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}

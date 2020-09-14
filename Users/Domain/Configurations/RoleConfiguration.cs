using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entity;
using Users.Domain.Models;

namespace Users.Domain.Configurations
{
    public class RoleConfiguration : BaseEntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(45);

            base.Configure(builder);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Migrations
{
    public class MigrationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MigrationConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Users.Domain.Configurations.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Users.Domain.Configurations.UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new Users.Domain.Configurations.RoleConfiguration());

            modelBuilder.ApplyConfiguration(new Pages.Domain.Configurations.PageConfiguration());
            modelBuilder.ApplyConfiguration(new Pages.Domain.Configurations.PageTranslationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

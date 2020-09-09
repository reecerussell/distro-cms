using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class PagesContext : DbContext
    {
        public PagesContext(DbContextOptions<PagesContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PageConfiguration());
            modelBuilder.ApplyConfiguration(new PageTranslationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

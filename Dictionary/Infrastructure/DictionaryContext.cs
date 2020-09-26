using Dictionary.Domain.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Infrastructure
{
    internal class DictionaryContext : DbContext
    {
        public DictionaryContext(DbContextOptions<DictionaryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SupportedCultureConfiguration());
            modelBuilder.ApplyConfiguration(new DictionaryItemConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

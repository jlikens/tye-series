using api.hat.Models;
using Microsoft.EntityFrameworkCore;

namespace api.hat.Data
{
    public partial class HatContext : DbContext
    {
        public HatContext(DbContextOptions<HatContext> options) : base(options)
        {
        }

        public DbSet<Hat> Hats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Hats");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hat>().ToTable("Hat");
            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
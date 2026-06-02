using Microsoft.EntityFrameworkCore;

namespace Homework3.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Journal> Journals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(AppContext.BaseDirectory, "app.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Journal>()
                .HasOne(j => j.Publisher)
                .WithMany(p => p.Journals)
                .HasForeignKey(j => j.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Publisher>()
                .HasIndex(p => p.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
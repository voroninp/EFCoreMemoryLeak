using Microsoft.EntityFrameworkCore;

namespace EFCoreMEmoryLeak
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<Entity> Entities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSqlLocalDB;Database=EFLeak;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity>().HasData(new Entity { Id = 1, Name = "Name" });
        }
    }
}

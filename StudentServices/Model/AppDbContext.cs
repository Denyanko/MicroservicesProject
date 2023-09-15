using Microsoft.EntityFrameworkCore;

namespace StudentServices.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Parent)
                .WithOne(p => p.Student)
                .HasForeignKey<Parent>(p => p.Id);
        }
    }
}

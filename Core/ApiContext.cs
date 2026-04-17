using CodeWithMe.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace CodeWithMe.Core
{
    public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<School> Schools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>().HasKey(school => school.SchoolId);
            modelBuilder.Entity<School>().Property(school => school.SchoolId).HasDefaultValueSql("UUID()");
        }
    }
}

using CodeWithMe.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CodeWithMe.Core
{
    public sealed class ApiContext : IdentityDbContext<User>
    {

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }
        public DbSet<Period> Periods { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IHasTimestamps>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Deleted;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ✅ Important!
            // Relation one-to-many
            modelBuilder.Entity<School>()
                .HasMany(s => s.Periods);
            //.WithOne(p => p.School)
            //.HasForeignKey(p => p.SchoolId);

            modelBuilder.Entity<School>()
                .Property(p => p.SchoolType)
                .HasConversion<string>();

            modelBuilder.Entity<Period>()
                .Property(p => p.PeriodType)
                .HasConversion<string>(); // 👈 convertit enum ↔ string

            //modelBuilder.Entity<Period>()
            //    .HasOne<School>();

        }
    }

    public interface IHasTimestamps
    {
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }

}

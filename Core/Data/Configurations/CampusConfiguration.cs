using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class CampusConfiguration : IEntityTypeConfiguration<Campus>
{
    public void Configure(EntityTypeBuilder<Campus> b)
    {
        b.ToTable("EduCampus");
        b.HasKey(c => c.Id);
        b.Property(c => c.Code).HasMaxLength(50).IsRequired();
        b.Property(c => c.Nom).HasMaxLength(200).IsRequired();
        b.Property(c => c.Adresse).HasMaxLength(500).IsRequired();
        b.Property(c => c.Ville).HasMaxLength(100).IsRequired();
        b.Property(c => c.TelephoneDirecteur).HasMaxLength(50);
        b.Property(c => c.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasMany(c => c.Salles).WithOne(s => s.Campus)
            .HasForeignKey(s => s.CampusId).OnDelete(DeleteBehavior.Restrict);
    }
}

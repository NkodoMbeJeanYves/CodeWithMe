using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class CycleConfiguration : IEntityTypeConfiguration<Cycle>
{
    public void Configure(EntityTypeBuilder<Cycle> b)
    {
        b.ToTable("EduCycles");
        b.HasKey(c => c.Id);
        b.Property(c => c.Libelle).HasMaxLength(200).IsRequired();
        b.Property(c => c.Code).HasMaxLength(20).IsRequired();
        b.Property(c => c.Type).HasMaxLength(30).IsRequired();
        b.Property(c => c.TypeFormation).HasMaxLength(30).IsRequired();
        b.Property(c => c.Description).HasMaxLength(1000);
        b.Property(c => c.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasMany(c => c.Filieres).WithOne(f => f.Cycle)
            .HasForeignKey(f => f.CycleId).OnDelete(DeleteBehavior.Restrict);
    }
}

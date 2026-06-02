using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class PeriodeEduConfiguration : IEntityTypeConfiguration<PeriodeEdu>
{
    public void Configure(EntityTypeBuilder<PeriodeEdu> b)
    {
        b.ToTable("EduPeriodes");
        b.HasKey(p => p.Id);
        b.Property(p => p.Libelle).HasMaxLength(100).IsRequired();
        b.Property(p => p.Type).HasMaxLength(30).IsRequired();
        b.Property(p => p.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasMany(p => p.Bulletins).WithOne(bl => bl.Periode)
            .HasForeignKey(bl => bl.PeriodeId).OnDelete(DeleteBehavior.Restrict);
    }
}

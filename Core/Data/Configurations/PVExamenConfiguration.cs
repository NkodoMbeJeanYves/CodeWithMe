using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class PVExamenConfiguration : IEntityTypeConfiguration<PVExamen>
{
    public void Configure(EntityTypeBuilder<PVExamen> b)
    {
        b.ToTable("EduPVExamens");
        b.HasKey(pv => pv.Id);
        b.HasIndex(pv => pv.EpreuveId).IsUnique();
        b.Property(pv => pv.Observations).HasMaxLength(5000);
        b.Property(pv => pv.SignePar).HasMaxLength(200);
        b.Property(pv => pv.Statut).HasMaxLength(20).HasDefaultValue("brouillon");
        b.Property(pv => pv.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(pv => pv.Epreuve).WithOne(e => e.PVExamen)
            .HasForeignKey<PVExamen>(pv => pv.EpreuveId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(pv => pv.CasFraude).WithOne(cf => cf.PVExamen)
            .HasForeignKey(cf => cf.PVExamenId).OnDelete(DeleteBehavior.Cascade);
    }
}

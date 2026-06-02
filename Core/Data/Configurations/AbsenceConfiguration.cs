using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class AbsenceConfiguration : IEntityTypeConfiguration<Absence>
{
    public void Configure(EntityTypeBuilder<Absence> b)
    {
        b.ToTable("EduAbsences");
        b.HasKey(a => a.Id);
        b.HasIndex(a => new { a.ApprenantId, a.Date, a.Statut });
        b.Property(a => a.DureeHeures).HasColumnType("decimal(4,2)");
        b.Property(a => a.Statut).HasMaxLength(20).HasDefaultValue("non_justifiee");
        b.Property(a => a.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(a => a.Apprenant).WithMany(ap => ap.Absences)
            .HasForeignKey(a => a.ApprenantId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(a => a.Justificatif).WithOne(j => j.Absence)
            .HasForeignKey<Justificatif>(j => j.AbsenceId).OnDelete(DeleteBehavior.Cascade);
    }
}

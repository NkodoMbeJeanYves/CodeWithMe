using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class AbsenceEnseignantConfiguration : IEntityTypeConfiguration<AbsenceEnseignant>
{
    public void Configure(EntityTypeBuilder<AbsenceEnseignant> b)
    {
        b.ToTable("EduAbsencesEnseignants");
        b.HasKey(a => a.Id);
        b.Property(a => a.Type).HasMaxLength(30).IsRequired();
        b.Property(a => a.Motif).HasMaxLength(500);
        b.Property(a => a.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(a => a.Enseignant).WithMany(e => e.Absences)
            .HasForeignKey(a => a.EnseignantId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(a => a.EnseignantRemplacant).WithMany()
            .HasForeignKey(a => a.EnseignantRemplacantId).IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

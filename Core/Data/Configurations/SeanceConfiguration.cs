using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class SeanceConfiguration : IEntityTypeConfiguration<Seance>
{
    public void Configure(EntityTypeBuilder<Seance> b)
    {
        b.ToTable("EduSeances");
        b.HasKey(s => s.Id);
        b.Property(s => s.TypeCours).HasMaxLength(20).IsRequired();
        b.Property(s => s.Statut).HasMaxLength(20).HasDefaultValue("planifiee");
        b.Property(s => s.ContenuEnseignant).HasMaxLength(5000);
        b.Property(s => s.TravauxDemandes).HasMaxLength(5000);
        b.Property(s => s.MotifAnnulation).HasMaxLength(1000);
        b.Property(s => s.MotifReport).HasMaxLength(1000);
        b.Property(s => s.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasIndex(s => new { s.EtablissementId, s.Date, s.SalleId });
        b.HasIndex(s => new { s.EtablissementId, s.Date, s.EnseignantId });
        b.HasOne(s => s.Enseignant).WithMany(e => e.Seances)
            .HasForeignKey(s => s.EnseignantId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(s => s.EnseignantRemplacant).WithMany()
            .HasForeignKey(s => s.EnseignantRemplacantId).IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        b.HasOne(s => s.Salle).WithMany(sa => sa.Seances)
            .HasForeignKey(s => s.SalleId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(s => s.CoursPlanifie).WithMany(cp => cp.Seances)
            .HasForeignKey(s => s.CoursPlanifieId).IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        b.HasMany(s => s.Presences).WithOne(p => p.Seance)
            .HasForeignKey(p => p.SeanceId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(s => s.Absences).WithOne(a => a.Seance)
            .HasForeignKey(a => a.SeanceId).OnDelete(DeleteBehavior.Restrict);
    }
}

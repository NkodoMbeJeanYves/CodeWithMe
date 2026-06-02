using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class EnseignantConfiguration : IEntityTypeConfiguration<Enseignant>
{
    public void Configure(EntityTypeBuilder<Enseignant> b)
    {
        b.ToTable("EduEnseignants");
        b.HasKey(e => e.Id);
        b.HasIndex(e => new { e.EtablissementId, e.Matricule }).IsUnique();
        b.HasIndex(e => e.Email).IsUnique();
        b.Property(e => e.Matricule).HasMaxLength(50).IsRequired();
        b.Property(e => e.Prenom).HasMaxLength(100).IsRequired();
        b.Property(e => e.Nom).HasMaxLength(100).IsRequired();
        b.Property(e => e.Email).HasMaxLength(200).IsRequired();
        b.Property(e => e.Telephone).HasMaxLength(50);
        b.Property(e => e.Genre).HasMaxLength(10).IsRequired();
        b.Property(e => e.PhotoUrl).HasMaxLength(500);
        b.Property(e => e.Statut).HasMaxLength(20).HasDefaultValue("actif");
        b.Property(e => e.TypeContrat).HasMaxLength(30).IsRequired();
        b.Property(e => e.NiveauDiplome).HasMaxLength(100);
        b.Property(e => e.TauxHoraire).HasColumnType("decimal(10,2)");
        b.Property(e => e.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasMany(e => e.Specialites).WithOne(s => s.Enseignant)
            .HasForeignKey(s => s.EnseignantId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Seances).WithOne(s => s.Enseignant)
            .HasForeignKey(s => s.EnseignantId).OnDelete(DeleteBehavior.Restrict);
    }
}

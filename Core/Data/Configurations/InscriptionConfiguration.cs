using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class InscriptionConfiguration : IEntityTypeConfiguration<Inscription>
{
    public void Configure(EntityTypeBuilder<Inscription> b)
    {
        b.ToTable("EduInscriptions");
        b.HasKey(i => i.Id);
        b.HasIndex(i => i.NumeroInscription).IsUnique();
        b.Property(i => i.Type).HasMaxLength(30).IsRequired();
        b.Property(i => i.Statut).HasMaxLength(20).HasDefaultValue("brouillon");
        b.Property(i => i.NumeroInscription).HasMaxLength(50).IsRequired();
        b.Property(i => i.ValidePar).HasMaxLength(200);
        b.Property(i => i.MotifRejet).HasMaxLength(1000);
        b.Property(i => i.Commentaire).HasMaxLength(2000);
        b.Property(i => i.FraisInscription).HasColumnType("decimal(12,2)");
        b.Property(i => i.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(i => i.Apprenant).WithMany(a => a.Inscriptions)
            .HasForeignKey(i => i.ApprenantId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(i => i.AnneeAcademique).WithMany(a => a.Inscriptions)
            .HasForeignKey(i => i.AnneeAcademiqueId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(i => i.ReinscriptionDepuis).WithMany()
            .HasForeignKey(i => i.ReinscriptionDepuisId).IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        b.HasOne(i => i.ListeAttenteEntry).WithOne(la => la.Inscription)
            .HasForeignKey<ListeAttente>(la => la.InscriptionId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(i => i.InscriptionGroupes).WithOne(ig => ig.Inscription)
            .HasForeignKey(ig => ig.InscriptionId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(i => i.InscriptionUEs).WithOne(iu => iu.Inscription)
            .HasForeignKey(iu => iu.InscriptionId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(i => i.Historique).WithOne(h => h.Inscription)
            .HasForeignKey(h => h.InscriptionId).OnDelete(DeleteBehavior.Cascade);
    }
}

using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class ApprenantConfiguration : IEntityTypeConfiguration<Apprenant>
{
    public void Configure(EntityTypeBuilder<Apprenant> b)
    {
        b.ToTable("EduApprenants");
        b.HasKey(a => a.Id);
        b.HasIndex(a => a.NumeroInscription).IsUnique();
        b.Property(a => a.NumeroInscription).HasMaxLength(50).IsRequired();
        b.Property(a => a.Type).HasMaxLength(20).IsRequired();
        b.Property(a => a.Nom).HasMaxLength(100).IsRequired();
        b.Property(a => a.Prenom).HasMaxLength(100).IsRequired();
        b.Property(a => a.LieuNaissance).HasMaxLength(200).IsRequired();
        b.Property(a => a.Genre).HasMaxLength(10).IsRequired();
        b.Property(a => a.Nationalite).HasMaxLength(100).IsRequired();
        b.Property(a => a.PhotoUrl).HasMaxLength(500);
        b.Property(a => a.Adresse).HasMaxLength(500).IsRequired();
        b.Property(a => a.Ville).HasMaxLength(100).IsRequired();
        b.Property(a => a.Pays).HasMaxLength(100).IsRequired();
        b.Property(a => a.Telephone).HasMaxLength(50);
        b.Property(a => a.Email).HasMaxLength(200);
        b.Property(a => a.Statut).HasMaxLength(20).HasDefaultValue("actif");
        b.Property(a => a.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasMany(a => a.Tuteurs).WithOne(t => t.Apprenant)
            .HasForeignKey(t => t.ApprenantId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(a => a.PiecesJustificatives).WithOne(p => p.Apprenant)
            .HasForeignKey(p => p.ApprenantId).OnDelete(DeleteBehavior.Cascade);
    }
}

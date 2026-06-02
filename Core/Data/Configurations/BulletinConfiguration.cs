using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class BulletinConfiguration : IEntityTypeConfiguration<Bulletin>
{
    public void Configure(EntityTypeBuilder<Bulletin> b)
    {
        b.ToTable("EduBulletins");
        b.HasKey(bl => bl.Id);
        b.HasIndex(bl => new { bl.ApprenantId, bl.PeriodeId }).IsUnique();
        b.Property(bl => bl.Type).HasMaxLength(20).HasDefaultValue("bulletin");
        b.Property(bl => bl.Statut).HasMaxLength(20).HasDefaultValue("brouillon");
        b.Property(bl => bl.MoyenneGenerale).HasColumnType("decimal(5,2)");
        b.Property(bl => bl.MoyenneClasse).HasColumnType("decimal(5,2)");
        b.Property(bl => bl.Mention).HasMaxLength(50);
        b.Property(bl => bl.Decision).HasMaxLength(100);
        b.Property(bl => bl.AppreciationGenerale).HasMaxLength(2000);
        b.Property(bl => bl.AppreciationProfPrincipal).HasMaxLength(2000);
        b.Property(bl => bl.SignePar).HasMaxLength(200);
        b.Property(bl => bl.PdfUrl).HasMaxLength(500);
        b.Property(bl => bl.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(bl => bl.Apprenant).WithMany(a => a.Bulletins)
            .HasForeignKey(bl => bl.ApprenantId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(bl => bl.Periode).WithMany(p => p.Bulletins)
            .HasForeignKey(bl => bl.PeriodeId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(bl => bl.Lignes).WithOne(l => l.Bulletin)
            .HasForeignKey(l => l.BulletinId).OnDelete(DeleteBehavior.Cascade);
    }
}

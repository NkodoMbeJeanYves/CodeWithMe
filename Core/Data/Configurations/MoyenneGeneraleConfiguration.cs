using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class MoyenneGeneraleConfiguration : IEntityTypeConfiguration<MoyenneGenerale>
{
    public void Configure(EntityTypeBuilder<MoyenneGenerale> b)
    {
        b.ToTable("EduMoyennesGenerales");
        b.HasKey(m => m.Id);
        b.HasIndex(m => new { m.ApprenantId, m.AnneeAcademiqueId, m.PeriodeId }).IsUnique();
        b.Property(m => m.Moyenne).HasColumnType("decimal(5,2)");
        b.Property(m => m.EctsAcquis).HasColumnType("decimal(5,1)");
        b.Property(m => m.EctsTotal).HasColumnType("decimal(5,1)");
        b.Property(m => m.Mention).HasMaxLength(50);
        b.Property(m => m.Statut).HasMaxLength(20).HasDefaultValue("en_cours");
        b.Property(m => m.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(m => m.Apprenant).WithMany(a => a.MoyennesGenerales)
            .HasForeignKey(m => m.ApprenantId).OnDelete(DeleteBehavior.Restrict);
    }
}

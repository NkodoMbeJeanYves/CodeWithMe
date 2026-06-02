using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class AnneeAcademiqueConfiguration : IEntityTypeConfiguration<AnneeAcademique>
{
    public void Configure(EntityTypeBuilder<AnneeAcademique> b)
    {
        b.ToTable("EduAnneesAcademiques");
        b.HasKey(a => a.Id);
        b.Property(a => a.Libelle).HasMaxLength(50).IsRequired();
        b.Property(a => a.Statut).HasMaxLength(30).HasDefaultValue("en_preparation");
        b.Property(a => a.TypePeriode).HasMaxLength(20);
        b.Property(a => a.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasMany(a => a.Periodes).WithOne(p => p.AnneeAcademique)
            .HasForeignKey(p => p.AnneeAcademiqueId).OnDelete(DeleteBehavior.Cascade);
    }
}

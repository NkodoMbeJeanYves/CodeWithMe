using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class ClasseEduConfiguration : IEntityTypeConfiguration<ClasseEdu>
{
    public void Configure(EntityTypeBuilder<ClasseEdu> b)
    {
        b.ToTable("EduClasses");
        b.HasKey(c => c.Id);
        b.Property(c => c.Libelle).HasMaxLength(200).IsRequired();
        b.Property(c => c.Code).HasMaxLength(20).IsRequired();
        b.Property(c => c.Statut).HasMaxLength(20).HasDefaultValue("active");
        b.Property(c => c.Salle).HasMaxLength(100);
        b.Property(c => c.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(c => c.Niveau).WithMany(n => n.Classes)
            .HasForeignKey(c => c.NiveauId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(c => c.Inscriptions).WithOne(i => i.Classe)
            .HasForeignKey(i => i.ClasseId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
    }
}

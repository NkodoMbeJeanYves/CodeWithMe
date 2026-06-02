using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class FiliereEduConfiguration : IEntityTypeConfiguration<FiliereEdu>
{
    public void Configure(EntityTypeBuilder<FiliereEdu> b)
    {
        b.ToTable("EduFilieres");
        b.HasKey(f => f.Id);
        b.Property(f => f.Libelle).HasMaxLength(200).IsRequired();
        b.Property(f => f.Code).HasMaxLength(20).IsRequired();
        b.Property(f => f.Description).HasMaxLength(1000);
        b.Property(f => f.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasMany(f => f.Niveaux).WithOne(n => n.Filiere)
            .HasForeignKey(n => n.FiliereId).OnDelete(DeleteBehavior.Restrict);
    }
}

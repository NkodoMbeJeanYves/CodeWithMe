using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class ConvocationConfiguration : IEntityTypeConfiguration<Convocation>
{
    public void Configure(EntityTypeBuilder<Convocation> b)
    {
        b.ToTable("EduConvocations");
        b.HasKey(c => c.Id);
        b.HasIndex(c => new { c.EpreuveId, c.ApprenantId }).IsUnique();
        b.Property(c => c.NumeroPlace).HasMaxLength(20);
        b.Property(c => c.Salle).HasMaxLength(100);
        b.Property(c => c.Statut).HasMaxLength(20).HasDefaultValue("generee");
        b.Property(c => c.MotifIneligibilite).HasMaxLength(500);
        b.Property(c => c.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(c => c.Apprenant).WithMany(a => a.Convocations)
            .HasForeignKey(c => c.ApprenantId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(c => c.Epreuve).WithMany(e => e.Convocations)
            .HasForeignKey(c => c.EpreuveId).OnDelete(DeleteBehavior.Cascade);
    }
}

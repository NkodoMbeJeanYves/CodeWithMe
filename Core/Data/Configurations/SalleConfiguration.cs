using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class SalleConfiguration : IEntityTypeConfiguration<Salle>
{
    public void Configure(EntityTypeBuilder<Salle> b)
    {
        b.ToTable("EduSalles");
        b.HasKey(s => s.Id);
        b.Property(s => s.Code).HasMaxLength(50).IsRequired();
        b.Property(s => s.Nom).HasMaxLength(200).IsRequired();
        b.Property(s => s.Type).HasMaxLength(30).IsRequired();
        b.Property(s => s.Statut).HasMaxLength(20).HasDefaultValue("disponible");
        b.Property(s => s.Batiment).HasMaxLength(100);
        b.Property(s => s.Etage).HasMaxLength(20);
        b.Property(s => s.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasMany(s => s.Equipements).WithOne(e => e.Salle)
            .HasForeignKey(e => e.SalleId).OnDelete(DeleteBehavior.Cascade);
    }
}

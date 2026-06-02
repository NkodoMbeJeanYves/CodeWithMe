using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> b)
    {
        b.ToTable("EduPromotions");
        b.HasKey(p => p.Id);
        b.Property(p => p.Libelle).HasMaxLength(200).IsRequired();
        b.Property(p => p.Code).HasMaxLength(20).IsRequired();
        b.Property(p => p.Statut).HasMaxLength(20).HasDefaultValue("active");
        b.Property(p => p.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(p => p.Niveau).WithMany(n => n.Promotions)
            .HasForeignKey(p => p.NiveauId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(p => p.Groupes).WithOne(g => g.Promotion)
            .HasForeignKey(g => g.PromotionId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(p => p.Inscriptions).WithOne(i => i.Promotion)
            .HasForeignKey(i => i.PromotionId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
    }
}

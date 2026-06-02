using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class PresenceConfiguration : IEntityTypeConfiguration<Presence>
{
    public void Configure(EntityTypeBuilder<Presence> b)
    {
        b.ToTable("EduPresences");
        b.HasKey(p => p.Id);
        b.HasIndex(p => new { p.SeanceId, p.ApprenantId }).IsUnique();
        b.Property(p => p.Statut).HasMaxLength(20).HasDefaultValue("present");
        b.Property(p => p.Remarque).HasMaxLength(500);
        b.Property(p => p.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(p => p.Apprenant).WithMany(a => a.Presences)
            .HasForeignKey(p => p.ApprenantId).OnDelete(DeleteBehavior.Restrict);
    }
}

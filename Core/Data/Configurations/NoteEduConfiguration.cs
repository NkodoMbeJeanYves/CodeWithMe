using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class NoteEduConfiguration : IEntityTypeConfiguration<NoteEdu>
{
    public void Configure(EntityTypeBuilder<NoteEdu> b)
    {
        b.ToTable("EduNotes");
        b.HasKey(n => n.Id);
        b.HasIndex(n => new { n.EvaluationId, n.ApprenantId }).IsUnique();
        b.Property(n => n.Valeur).HasColumnType("decimal(5,2)");
        b.Property(n => n.Statut).HasMaxLength(20).HasDefaultValue("brouillon");
        b.Property(n => n.Commentaire).HasMaxLength(2000);
        b.Property(n => n.MotifModification).HasMaxLength(1000);
        b.Property(n => n.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(n => n.Evaluation).WithMany(e => e.Notes)
            .HasForeignKey(n => n.EvaluationId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(n => n.Apprenant).WithMany(a => a.Notes)
            .HasForeignKey(n => n.ApprenantId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(n => n.Historique).WithOne(h => h.Note)
            .HasForeignKey(h => h.NoteId).OnDelete(DeleteBehavior.Cascade);
    }
}

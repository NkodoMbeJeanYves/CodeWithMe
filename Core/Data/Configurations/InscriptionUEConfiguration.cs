using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class InscriptionUEConfiguration : IEntityTypeConfiguration<InscriptionUE>
{
    public void Configure(EntityTypeBuilder<InscriptionUE> b)
    {
        b.ToTable("EduInscriptionUEs");
        b.HasKey(iu => iu.Id);
        b.HasIndex(iu => new { iu.InscriptionId, iu.UeId }).IsUnique();
        b.Property(iu => iu.Statut).HasMaxLength(20).HasDefaultValue("inscrit");
        b.Property(iu => iu.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
        b.HasOne(iu => iu.Ue).WithMany(ue => ue.InscriptionUEs)
            .HasForeignKey(iu => iu.UeId).OnDelete(DeleteBehavior.Restrict);
    }
}

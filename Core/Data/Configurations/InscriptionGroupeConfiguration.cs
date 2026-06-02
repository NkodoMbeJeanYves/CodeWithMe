using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class InscriptionGroupeConfiguration : IEntityTypeConfiguration<InscriptionGroupe>
{
    public void Configure(EntityTypeBuilder<InscriptionGroupe> b)
    {
        b.ToTable("EduInscriptionGroupes");
        b.HasKey(ig => new { ig.InscriptionId, ig.GroupeId });
        b.HasOne(ig => ig.Groupe).WithMany(g => g.InscriptionGroupes)
            .HasForeignKey(ig => ig.GroupeId).OnDelete(DeleteBehavior.Restrict);
    }
}

using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class NiveauConfiguration : IEntityTypeConfiguration<Niveau>
{
    public void Configure(EntityTypeBuilder<Niveau> b)
    {
        b.ToTable("EduNiveaux");
        b.HasKey(n => n.Id);
        b.Property(n => n.Libelle).HasMaxLength(200).IsRequired();
        b.Property(n => n.Code).HasMaxLength(20).IsRequired();
        b.Property(n => n.TypeFormation).HasMaxLength(30).IsRequired();
        b.Property(n => n.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
    }
}

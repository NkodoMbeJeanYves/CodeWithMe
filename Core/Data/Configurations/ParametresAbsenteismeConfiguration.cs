using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class ParametresAbsenteismeConfiguration : IEntityTypeConfiguration<ParametresAbsenteisme>
{
    public void Configure(EntityTypeBuilder<ParametresAbsenteisme> b)
    {
        b.ToTable("EduParametresAbsenteisme");
        b.HasKey(p => p.Id);
        b.HasIndex(p => p.EtablissementId).IsUnique();
        b.Property(p => p.CreatedAt).HasDefaultValueSql("UTC_TIMESTAMP()");
    }
}

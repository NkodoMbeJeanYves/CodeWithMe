using CodeWithMe.Core.Models.Edu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMe.Core.Data.Configurations;

public class SalleEquipementConfiguration : IEntityTypeConfiguration<SalleEquipement>
{
    public void Configure(EntityTypeBuilder<SalleEquipement> b)
    {
        b.ToTable("EduSalleEquipements");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).ValueGeneratedOnAdd();
        b.Property(e => e.Nom).HasMaxLength(100).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
    }
}

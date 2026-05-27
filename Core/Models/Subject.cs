using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models
{
    [Table("Subjects")]
    public class Subject : HasTimestamps, ITenantScoped
    {
        [Column("subject_id")]
        [Key]
        public required string SubjectId { get; set; }

        [Column("tenant_id")]
        [MaxLength(36)]
        public string TenantId { get; set; } = Tenant.DefaultTenantId;

        [Column("subject_name")]
        public required string SubjectName { get; set; }

        [Column("description")]
        public string? Description { get; set; }
    }
}

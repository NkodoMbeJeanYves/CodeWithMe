using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models
{
    [Table("CWM_Subjects")]
    public class Subject : IHasTimestamps
    {
        [Column("subject_id")]
        [Key]
        public required string SubjectId { get; set; }

        [Column("subject_name")]
        public required string SubjectName { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}

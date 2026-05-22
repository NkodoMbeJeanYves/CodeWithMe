using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models
{
    [Table("Subjects")]
    public class Subject : HasTimestamps
    {
        [Column("subject_id")]
        [Key]
        public required string SubjectId { get; set; }

        [Column("subject_name")]
        public required string SubjectName { get; set; }

        [Column("description")]
        public string? Description { get; set; }
    }
}

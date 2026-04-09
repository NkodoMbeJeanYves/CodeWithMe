using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models
{
    [PrimaryKey(nameof(Subject.SubjectId))]
    public class Subject
    {
        [Column("subject_id")]
        public required string SubjectId { get; set; }
        [Column("subject_name")]
        public required string SubjectName { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt  { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models
{
    [PrimaryKey(nameof(Subject.SubjectId))]
    [Table("CWM_Subjects")]
    public class Subject
    {
        [Column("subject_id")]
        [Required]
        [StringLength(20, ErrorMessage = "Name field must be lower than 20 characters", MinimumLength = 5)]
        public required string SubjectId { get; set; }


        [Column("subject_name")]
        [Required]
        [StringLength(20, ErrorMessage = "Name field must be lower than 20 characters", MinimumLength = 5)]
        public required string SubjectName { get; set; }

        [Column("description")]
        [Required]
        [StringLength(20, ErrorMessage = "Name field must be lower than 20 characters", MinimumLength = 5)]
        public string? Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt  { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public record SubjectDto(
        [Required]
        [StringLength(20, ErrorMessage = "Name field must be lower than 20 characters", MinimumLength = 5)]
        string SubjectId,


        [Required]
        [StringLength(20, ErrorMessage = "Name field must be lower than 20 characters", MinimumLength = 5)]
        string SubjectName,


        [Required]
        [StringLength(20, ErrorMessage = "Name field must be lower than 20 characters", MinimumLength = 5)]
        string? Description
    );
    
}

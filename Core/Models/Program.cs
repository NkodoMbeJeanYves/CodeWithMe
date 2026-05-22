using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("programs")]
public class ProgramModel : IHasTimestamps
{
    [Key]
    public required string Program_id { get; set; }

    [Column("program_name")]
    public required string Name { get; set; }

    [ForeignKey("School")]
    [Column("school_id")]
    public string SchoolId { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [NotMapped]
    public DateTime? DeletedAt { get; set; }
}

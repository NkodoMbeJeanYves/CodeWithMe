using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[NotMapped]
public class HasTimestamps : IHasTimestamps
{
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}

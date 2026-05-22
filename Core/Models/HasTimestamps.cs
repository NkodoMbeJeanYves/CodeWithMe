using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[NotMapped]
public class HasTimestamps : IHasTimestamps
{
    [Column("created_at")]
    public DateTime? CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [Column("deleted_at")]
    public DateTime? DeletedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

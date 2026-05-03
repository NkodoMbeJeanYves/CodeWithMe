namespace CodeWithMe.Core.Models;

public class RevokedToken
{
    public int Id { get; set; }
    public required string Jti { get; set; }
    public DateTime RevokedAt { get; set; }
}

namespace CodeWithMe.Core.Models;

public class RefreshToken
{
    public int Id { get; set; } // auto PK by convention
    public required string Token { get; set; }
    public required string UserName { get; set; }
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; } = false;
}

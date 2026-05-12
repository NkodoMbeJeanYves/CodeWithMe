namespace CodeWithMe.Core.Models;

//[NotMapped] EF Core wont create its table, but we want it to be stored in the database, so we remove this attribute
public class RevokedToken
{
    public int Id { get; set; }
    public required string Jti { get; set; }
    public DateTime RevokedAt { get; set; }
}

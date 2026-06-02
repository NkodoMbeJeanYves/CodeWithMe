namespace CodeWithMe.Core.Models.Edu;

public class DeliberationMembre
{
    public int Id { get; set; }
    public Guid DeliberationId { get; set; }
    public string Membre { get; set; } = null!;

    public Deliberation Deliberation { get; set; } = null!;
}

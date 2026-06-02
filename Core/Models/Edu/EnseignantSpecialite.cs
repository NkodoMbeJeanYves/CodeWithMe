namespace CodeWithMe.Core.Models.Edu;

public class EnseignantSpecialite
{
    public int Id { get; set; }
    public Guid EnseignantId { get; set; }
    public string Libelle { get; set; } = null!;

    public Enseignant Enseignant { get; set; } = null!;
}

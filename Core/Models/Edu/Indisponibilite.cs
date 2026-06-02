namespace CodeWithMe.Core.Models.Edu;

public class Indisponibilite : BaseEntity
{
    public Guid EnseignantId { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public TimeOnly? HeureDebut { get; set; }
    public TimeOnly? HeureFin { get; set; }
    public string? Motif { get; set; }
    public string Type { get; set; } = null!;

    public Enseignant Enseignant { get; set; } = null!;
}

namespace CodeWithMe.Core.Models.Edu;

public class AbsenceEnseignant : BaseEntity
{
    public Guid EnseignantId { get; set; }
    public Guid? EnseignantRemplacantId { get; set; }
    public string Type { get; set; } = null!;
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public string? Motif { get; set; }
    public bool RemplacementPrevu { get; set; } = false;

    public Enseignant Enseignant { get; set; } = null!;
    public Enseignant? EnseignantRemplacant { get; set; }
}

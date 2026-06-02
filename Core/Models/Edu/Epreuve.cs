namespace CodeWithMe.Core.Models.Edu;

public class Epreuve : BaseEntity
{
    public Guid SessionId { get; set; }
    public Guid MatiereId { get; set; }
    public Guid? ClasseId { get; set; }
    public Guid? PromotionId { get; set; }
    public Guid SalleId { get; set; }
    public Guid? EnseignantSurveillantId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly HeureDebut { get; set; }
    public TimeOnly HeureFin { get; set; }
    public int DureeMinutes { get; set; }
    public decimal Coefficient { get; set; }
    public decimal NoteMax { get; set; } = 20;
    public bool ConvocationsGenerees { get; set; } = false;

    public SessionExamen Session { get; set; } = null!;
    public Matiere Matiere { get; set; } = null!;
    public Salle Salle { get; set; } = null!;
    public Enseignant? EnseignantSurveillant { get; set; }
    public ICollection<Convocation> Convocations { get; set; } = [];
    public PVExamen? PVExamen { get; set; }
}

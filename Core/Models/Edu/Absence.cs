namespace CodeWithMe.Core.Models.Edu;

public class Absence : BaseEntity
{
    public Guid ApprenantId { get; set; }
    public Guid SeanceId { get; set; }
    public Guid? MatiereId { get; set; }
    public Guid? EnseignantId { get; set; }
    public Guid? ClasseId { get; set; }
    public Guid? PromotionId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly HeureDebut { get; set; }
    public TimeOnly HeureFin { get; set; }
    public decimal DureeHeures { get; set; }
    public string Statut { get; set; } = "non_justifiee";
    public bool EstExamen { get; set; } = false;
    public bool ImpactNote { get; set; } = false;
    public bool NotifieeParent { get; set; } = false;
    public DateTime? DateNotification { get; set; }

    public Apprenant Apprenant { get; set; } = null!;
    public Seance Seance { get; set; } = null!;
    public Justificatif? Justificatif { get; set; }
}

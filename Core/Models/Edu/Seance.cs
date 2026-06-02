namespace CodeWithMe.Core.Models.Edu;

public class Seance : BaseEntity
{
    public Guid? CoursPlanifieId { get; set; }
    public Guid EtablissementId { get; set; }
    public Guid MatiereId { get; set; }
    public Guid EnseignantId { get; set; }
    public Guid? ClasseId { get; set; }
    public Guid? PromotionId { get; set; }
    public Guid? GroupeId { get; set; }
    public Guid SalleId { get; set; }
    public string TypeCours { get; set; } = null!;
    public DateOnly Date { get; set; }
    public TimeOnly HeureDebut { get; set; }
    public TimeOnly HeureFin { get; set; }
    public int DureeMinutes { get; set; }
    public string Statut { get; set; } = "planifiee";
    public string? ContenuEnseignant { get; set; }
    public string? TravauxDemandes { get; set; }
    public bool PresencesSaisies { get; set; } = false;
    public bool EstRemplacement { get; set; } = false;
    public Guid? EnseignantRemplacantId { get; set; }
    public string? MotifAnnulation { get; set; }
    public string? MotifReport { get; set; }
    public DateOnly? DateReport { get; set; }

    public CoursPlanifie? CoursPlanifie { get; set; }
    public Matiere Matiere { get; set; } = null!;
    public Enseignant Enseignant { get; set; } = null!;
    public Enseignant? EnseignantRemplacant { get; set; }
    public Salle Salle { get; set; } = null!;
    public ClasseEdu? Classe { get; set; }
    public Promotion? Promotion { get; set; }
    public Groupe? Groupe { get; set; }
    public ICollection<Presence> Presences { get; set; } = [];
    public ICollection<Absence> Absences { get; set; } = [];
}

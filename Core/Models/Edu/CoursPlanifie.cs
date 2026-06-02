namespace CodeWithMe.Core.Models.Edu;

public class CoursPlanifie : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public Guid? PeriodeId { get; set; }
    public Guid MatiereId { get; set; }
    public Guid EnseignantId { get; set; }
    public Guid? ClasseId { get; set; }
    public Guid? PromotionId { get; set; }
    public Guid? GroupeId { get; set; }
    public Guid SalleId { get; set; }
    public Guid? CreneauId { get; set; }
    public string TypeCours { get; set; } = null!;
    public string TypeRecurrence { get; set; } = null!;
    public int JourSemaine { get; set; }
    public TimeOnly HeureDebut { get; set; }
    public TimeOnly HeureFin { get; set; }
    public DateOnly DateDebutValidite { get; set; }
    public DateOnly DateFinValidite { get; set; }
    public string? Couleur { get; set; }
    public string? Note { get; set; }
    public string Statut { get; set; } = "actif";

    public Matiere Matiere { get; set; } = null!;
    public Enseignant Enseignant { get; set; } = null!;
    public Salle Salle { get; set; } = null!;
    public ICollection<Seance> Seances { get; set; } = [];
}

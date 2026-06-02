namespace CodeWithMe.Core.Models.Edu;

public class Enseignant : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public string Matricule { get; set; } = null!;
    public string Prenom { get; set; } = null!;
    public string Nom { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Telephone { get; set; }
    public string Genre { get; set; } = null!;
    public DateOnly? DateNaissance { get; set; }
    public string? Adresse { get; set; }
    public string? PhotoUrl { get; set; }
    public string Statut { get; set; } = "actif";
    public string TypeContrat { get; set; } = null!;
    public DateOnly DateEntree { get; set; }
    public DateOnly? DateSortie { get; set; }
    public string? NiveauDiplome { get; set; }
    public int? ChargeHoraireMax { get; set; }
    public decimal? TauxHoraire { get; set; }

    public ICollection<EnseignantSpecialite> Specialites { get; set; } = [];
    public ICollection<AffectationMatiere> Affectations { get; set; } = [];
    public ICollection<Seance> Seances { get; set; } = [];
    public ICollection<Evaluation> Evaluations { get; set; } = [];
    public ICollection<Indisponibilite> Indisponibilites { get; set; } = [];
    public ICollection<AbsenceEnseignant> Absences { get; set; } = [];
}

namespace CodeWithMe.Core.Models.Edu;

public class Inscription : BaseEntity
{
    public Guid ApprenantId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public Guid EtablissementId { get; set; }
    public string Type { get; set; } = null!;
    public string Statut { get; set; } = "brouillon";
    public Guid? ClasseId { get; set; }
    public Guid? PromotionId { get; set; }
    public string NumeroInscription { get; set; } = null!;
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;
    public DateTime? DateLimiteValidation { get; set; }
    public DateTime? DateValidation { get; set; }
    public string? ValidePar { get; set; }
    public string? MotifRejet { get; set; }
    public decimal? FraisInscription { get; set; }
    public bool FraisPayes { get; set; } = false;
    public bool ListAttente { get; set; } = false;
    public int? PositionListeAttente { get; set; }
    public string? Commentaire { get; set; }
    public Guid? ReinscriptionDepuisId { get; set; }

    public Apprenant Apprenant { get; set; } = null!;
    public AnneeAcademique AnneeAcademique { get; set; } = null!;
    public ClasseEdu? Classe { get; set; }
    public Promotion? Promotion { get; set; }
    public Inscription? ReinscriptionDepuis { get; set; }
    public ICollection<InscriptionGroupe> InscriptionGroupes { get; set; } = [];
    public ICollection<InscriptionUE> InscriptionUEs { get; set; } = [];
    public ICollection<InscriptionHistorique> Historique { get; set; } = [];
    public ListeAttente? ListeAttenteEntry { get; set; }
}

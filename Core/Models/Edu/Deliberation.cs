namespace CodeWithMe.Core.Models.Edu;

public class Deliberation : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public Guid PeriodeId { get; set; }
    public Guid ClasseOuPromotionId { get; set; }
    public string ClasseOuPromotionLibelle { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Statut { get; set; } = "planifiee";
    public string Session { get; set; } = null!;
    public DateTime DateDeliberation { get; set; }
    public string? President { get; set; }
    public bool CompensationActivee { get; set; } = false;
    public string? PvUrl { get; set; }
    public string? SignePar { get; set; }
    public DateTime? DateSigne { get; set; }
    public DateTime? DatePublication { get; set; }

    public ICollection<DeliberationMembre> Membres { get; set; } = [];
    public ICollection<DeliberationLigne> Lignes { get; set; } = [];
}

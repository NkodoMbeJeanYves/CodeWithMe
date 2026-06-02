namespace CodeWithMe.Core.Models.Edu;

public class Promotion : BaseEntity
{
    public Guid NiveauId { get; set; }
    public Guid FiliereId { get; set; }
    public Guid EtablissementId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public Guid? ResponsableId { get; set; }
    public string Libelle { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int CapaciteMax { get; set; }
    public int EffectifActuel { get; set; } = 0;
    public string Statut { get; set; } = "active";

    public Niveau Niveau { get; set; } = null!;
    public ICollection<Groupe> Groupes { get; set; } = [];
    public ICollection<Inscription> Inscriptions { get; set; } = [];
}

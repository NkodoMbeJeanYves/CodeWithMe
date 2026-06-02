namespace CodeWithMe.Core.Models.Edu;

public class Groupe : BaseEntity
{
    public Guid PromotionId { get; set; }
    public Guid? EnseignantId { get; set; }
    public string Libelle { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int CapaciteMax { get; set; }
    public int EffectifActuel { get; set; } = 0;
    public bool Actif { get; set; } = true;

    public Promotion Promotion { get; set; } = null!;
    public ICollection<InscriptionGroupe> InscriptionGroupes { get; set; } = [];
}

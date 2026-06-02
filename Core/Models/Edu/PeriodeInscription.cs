namespace CodeWithMe.Core.Models.Edu;

public class PeriodeInscription : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public string Libelle { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime DateOuverture { get; set; }
    public DateTime DateCloture { get; set; }
    public bool Ouverte { get; set; } = false;
    public int? CapaciteMax { get; set; }
}

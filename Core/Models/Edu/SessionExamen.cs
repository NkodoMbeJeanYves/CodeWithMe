namespace CodeWithMe.Core.Models.Edu;

public class SessionExamen : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public Guid? PeriodeId { get; set; }
    public string Libelle { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Statut { get; set; } = "planifiee";
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }

    public ICollection<Epreuve> Epreuves { get; set; } = [];
}

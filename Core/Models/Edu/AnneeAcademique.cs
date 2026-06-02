namespace CodeWithMe.Core.Models.Edu;

public class AnneeAcademique : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public string Libelle { get; set; } = null!;
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public string Statut { get; set; } = "en_preparation";
    public string? TypePeriode { get; set; }
    public bool Active { get; set; } = false;

    public ICollection<PeriodeEdu> Periodes { get; set; } = [];
    public ICollection<Inscription> Inscriptions { get; set; } = [];
}

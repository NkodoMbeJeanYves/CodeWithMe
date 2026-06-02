namespace CodeWithMe.Core.Models.Edu;

public class PeriodeEdu : BaseEntity
{
    public Guid AnneeAcademiqueId { get; set; }
    public string Libelle { get; set; } = null!;
    public int Numero { get; set; }
    public string Type { get; set; } = null!;
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public bool Active { get; set; } = false;

    public AnneeAcademique AnneeAcademique { get; set; } = null!;
    public ICollection<Bulletin> Bulletins { get; set; } = [];
}

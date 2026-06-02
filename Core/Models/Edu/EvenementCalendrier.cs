namespace CodeWithMe.Core.Models.Edu;

public class EvenementCalendrier : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public Guid? AnneeAcademiqueId { get; set; }
    public string Titre { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public string Type { get; set; } = null!;
}

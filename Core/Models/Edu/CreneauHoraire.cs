namespace CodeWithMe.Core.Models.Edu;

public class CreneauHoraire : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public string Libelle { get; set; } = null!;
    public TimeOnly HeureDebut { get; set; }
    public TimeOnly HeureFin { get; set; }
    public int DureeMinutes { get; set; }
    public int Ordre { get; set; }
    public bool Actif { get; set; } = true;
}

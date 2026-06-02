namespace CodeWithMe.Core.Models.Edu;

public class Salle : BaseEntity
{
    public Guid CampusId { get; set; }
    public string Code { get; set; } = null!;
    public string Nom { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Capacite { get; set; }
    public string Statut { get; set; } = "disponible";
    public string? Batiment { get; set; }
    public string? Etage { get; set; }

    public Campus Campus { get; set; } = null!;
    public ICollection<SalleEquipement> Equipements { get; set; } = [];
    public ICollection<Seance> Seances { get; set; } = [];
}

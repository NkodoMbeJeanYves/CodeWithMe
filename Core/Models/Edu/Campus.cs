namespace CodeWithMe.Core.Models.Edu;

public class Campus : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public string Code { get; set; } = null!;
    public string Nom { get; set; } = null!;
    public string Adresse { get; set; } = null!;
    public string Ville { get; set; } = null!;
    public string? TelephoneDirecteur { get; set; }
    public bool Principal { get; set; } = false;
    public bool Actif { get; set; } = true;

    public ICollection<Salle> Salles { get; set; } = [];
}

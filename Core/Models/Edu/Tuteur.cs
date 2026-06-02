namespace CodeWithMe.Core.Models.Edu;

public class Tuteur : BaseEntity
{
    public Guid ApprenantId { get; set; }
    public string Nom { get; set; } = null!;
    public string Prenom { get; set; } = null!;
    public string LienParente { get; set; } = null!;
    public string Telephone { get; set; } = null!;
    public string? TelephoneSecondaire { get; set; }
    public string? Email { get; set; }
    public string? Adresse { get; set; }
    public string? Profession { get; set; }
    public bool ContactPrincipal { get; set; } = false;
    public bool AccesPortail { get; set; } = false;

    public Apprenant Apprenant { get; set; } = null!;
}

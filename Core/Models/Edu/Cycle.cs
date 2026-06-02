namespace CodeWithMe.Core.Models.Edu;

public class Cycle : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public string Libelle { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string TypeFormation { get; set; } = null!;
    public string? Description { get; set; }
    public bool Actif { get; set; } = true;
    public int Ordre { get; set; }

    public ICollection<FiliereEdu> Filieres { get; set; } = [];
}

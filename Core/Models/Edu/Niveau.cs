namespace CodeWithMe.Core.Models.Edu;

public class Niveau : BaseEntity
{
    public Guid FiliereId { get; set; }
    public string Libelle { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int Ordre { get; set; }
    public string TypeFormation { get; set; } = null!;
    public bool Actif { get; set; } = true;

    public FiliereEdu Filiere { get; set; } = null!;
    public ICollection<ClasseEdu> Classes { get; set; } = [];
    public ICollection<Promotion> Promotions { get; set; } = [];
    public ICollection<UniteEnseignement> UniteEnseignements { get; set; } = [];
}

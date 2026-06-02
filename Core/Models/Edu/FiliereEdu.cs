namespace CodeWithMe.Core.Models.Edu;

public class FiliereEdu : BaseEntity
{
    public Guid CycleId { get; set; }
    public Guid EtablissementId { get; set; }
    public string Libelle { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    public bool SystemeLMD { get; set; } = false;
    public int DureeAnnees { get; set; }
    public bool Actif { get; set; } = true;

    public Cycle Cycle { get; set; } = null!;
    public ICollection<Niveau> Niveaux { get; set; } = [];
    public ICollection<Matiere> Matieres { get; set; } = [];
    public ICollection<UniteEnseignement> UniteEnseignements { get; set; } = [];
}

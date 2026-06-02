namespace CodeWithMe.Core.Models.Edu;

public class UniteEnseignement : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public Guid FiliereId { get; set; }
    public Guid NiveauId { get; set; }
    public Guid? AnneeAcademiqueId { get; set; }
    public string Semestre { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Libelle { get; set; } = null!;
    public string Type { get; set; } = null!;
    public decimal Credits { get; set; }
    public decimal Coefficient { get; set; }
    public int VolHoraireTotal { get; set; }
    public string NatureEvaluation { get; set; } = null!;
    public decimal PonderationCC { get; set; }
    public decimal PonderationExamen { get; set; }
    public bool Eliminatoire { get; set; } = false;
    public decimal SeuilValidation { get; set; }
    public bool Compensable { get; set; } = true;
    public bool Actif { get; set; } = true;

    public FiliereEdu Filiere { get; set; } = null!;
    public Niveau Niveau { get; set; } = null!;
    public ICollection<Matiere> Matieres { get; set; } = [];
    public ICollection<InscriptionUE> InscriptionUEs { get; set; } = [];
}

namespace CodeWithMe.Core.Models.Edu;

public class Matiere : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public Guid? FiliereId { get; set; }
    public Guid? NiveauId { get; set; }
    public Guid? UeId { get; set; }
    public Guid? AnneeAcademiqueId { get; set; }
    public string Code { get; set; } = null!;
    public string Libelle { get; set; } = null!;
    public string Type { get; set; } = null!;
    public decimal Coefficient { get; set; }
    public int VolHoraireCM { get; set; }
    public int VolHoraireTD { get; set; }
    public int VolHoraireTP { get; set; }
    public int VolHoraireTotal { get; set; }
    public string NatureEvaluation { get; set; } = null!;
    public decimal PonderationCC { get; set; }
    public decimal PonderationExamen { get; set; }
    public bool Eliminatoire { get; set; } = false;
    public decimal? SeuilEliminatoire { get; set; }
    public decimal NoteMax { get; set; } = 20;
    public bool Actif { get; set; } = true;

    public FiliereEdu? Filiere { get; set; }
    public UniteEnseignement? Ue { get; set; }
    public ICollection<AffectationMatiere> Affectations { get; set; } = [];
    public ICollection<Evaluation> Evaluations { get; set; } = [];
    public ICollection<CoursPlanifie> CoursPlanifies { get; set; } = [];
}

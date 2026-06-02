namespace CodeWithMe.Core.Models.Edu;

public class Evaluation : BaseEntity
{
    public Guid MatiereId { get; set; }
    public Guid PeriodeId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public Guid? ClasseId { get; set; }
    public Guid? PromotionId { get; set; }
    public Guid EnseignantId { get; set; }
    public string Intitule { get; set; } = null!;
    public string Type { get; set; } = null!;
    public decimal Ponderation { get; set; }
    public decimal Coefficient { get; set; }
    public decimal NoteMax { get; set; } = 20;
    public DateTime DateEvaluation { get; set; }
    public string Statut { get; set; } = "brouillon";

    public Matiere Matiere { get; set; } = null!;
    public Enseignant Enseignant { get; set; } = null!;
    public ICollection<NoteEdu> Notes { get; set; } = [];
}

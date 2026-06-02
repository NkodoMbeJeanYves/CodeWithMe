namespace CodeWithMe.Core.Models.Edu;

public class NoteEdu : BaseEntity
{
    public Guid EvaluationId { get; set; }
    public Guid ApprenantId { get; set; }
    public decimal? Valeur { get; set; }
    public bool Absent { get; set; } = false;
    public bool Dispense { get; set; } = false;
    public string? Commentaire { get; set; }
    public string Statut { get; set; } = "brouillon";
    public Guid? SaisieParId { get; set; }
    public Guid? ValideParId { get; set; }
    public string? MotifModification { get; set; }

    public Evaluation Evaluation { get; set; } = null!;
    public Apprenant Apprenant { get; set; } = null!;
    public ICollection<NoteHistorique> Historique { get; set; } = [];
}

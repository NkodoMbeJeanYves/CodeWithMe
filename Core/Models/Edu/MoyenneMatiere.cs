namespace CodeWithMe.Core.Models.Edu;

public class MoyenneMatiere : BaseEntity
{
    public Guid MatiereId { get; set; }
    public Guid ApprenantId { get; set; }
    public Guid PeriodeId { get; set; }
    public decimal Moyenne { get; set; }
    public decimal? NoteCC { get; set; }
    public decimal? NotePartiel { get; set; }
    public decimal? NoteExamen { get; set; }
    public string? AppreciationEnseignant { get; set; }
    public bool Eliminatoire { get; set; } = false;
    public decimal? SeuilEliminatoire { get; set; }
    public string Statut { get; set; } = "en_cours";

    public Matiere Matiere { get; set; } = null!;
    public Apprenant Apprenant { get; set; } = null!;
}

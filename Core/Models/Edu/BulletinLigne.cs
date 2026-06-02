namespace CodeWithMe.Core.Models.Edu;

public class BulletinLigne : BaseEntity
{
    public Guid BulletinId { get; set; }
    public Guid MatiereId { get; set; }
    public decimal Coefficient { get; set; }
    public decimal? NoteCC { get; set; }
    public decimal? NotePartiel { get; set; }
    public decimal? NoteExamen { get; set; }
    public decimal? Moyenne { get; set; }
    public decimal? MoyenneClasse { get; set; }
    public string? Appreciation { get; set; }
    public string? EnseignantNom { get; set; }
    public int? Rang { get; set; }
    public bool Eliminatoire { get; set; } = false;

    public Bulletin Bulletin { get; set; } = null!;
    public Matiere Matiere { get; set; } = null!;
}

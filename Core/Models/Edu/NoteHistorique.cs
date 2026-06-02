namespace CodeWithMe.Core.Models.Edu;

public class NoteHistorique : BaseEntity
{
    public Guid NoteId { get; set; }
    public decimal? AncienneValeur { get; set; }
    public decimal? NouvelleValeur { get; set; }
    public string ModifiePar { get; set; } = null!;
    public string? Motif { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public NoteEdu Note { get; set; } = null!;
}

namespace CodeWithMe.Core.Models.Edu;

public class InscriptionHistorique : BaseEntity
{
    public Guid InscriptionId { get; set; }
    public string Statut { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Par { get; set; } = null!;
    public string? Commentaire { get; set; }

    public Inscription Inscription { get; set; } = null!;
}

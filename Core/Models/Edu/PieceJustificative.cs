namespace CodeWithMe.Core.Models.Edu;

public class PieceJustificative : BaseEntity
{
    public Guid ApprenantId { get; set; }
    public string Type { get; set; } = null!;
    public string Nom { get; set; } = null!;
    public string FichierUrl { get; set; } = null!;
    public string Statut { get; set; } = "en_attente";
    public string? Commentaire { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ValidatedAt { get; set; }

    public Apprenant Apprenant { get; set; } = null!;
}

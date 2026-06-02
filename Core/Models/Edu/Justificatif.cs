namespace CodeWithMe.Core.Models.Edu;

public class Justificatif : BaseEntity
{
    public Guid AbsenceId { get; set; }
    public string Type { get; set; } = null!;
    public string? Description { get; set; }
    public string? FichierUrl { get; set; }
    public string? FichierNom { get; set; }
    public Guid SoumisParId { get; set; }
    public DateTime DateSoumission { get; set; } = DateTime.UtcNow;
    public Guid? ValidateParId { get; set; }
    public DateTime? DateValidation { get; set; }
    public string? CommentaireValidation { get; set; }
    public string Statut { get; set; } = "en_attente";

    public Absence Absence { get; set; } = null!;
}

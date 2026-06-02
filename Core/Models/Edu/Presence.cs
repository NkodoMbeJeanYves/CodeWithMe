namespace CodeWithMe.Core.Models.Edu;

public class Presence : BaseEntity
{
    public Guid SeanceId { get; set; }
    public Guid ApprenantId { get; set; }
    public string Statut { get; set; } = "present";
    public int? MinutesRetard { get; set; }
    public string? Remarque { get; set; }
    public Guid? SaisieParId { get; set; }

    public Seance Seance { get; set; } = null!;
    public Apprenant Apprenant { get; set; } = null!;
}

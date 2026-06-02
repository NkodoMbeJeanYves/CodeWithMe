namespace CodeWithMe.Core.Models.Edu;

public class PVExamen : BaseEntity
{
    public Guid EpreuveId { get; set; }
    public DateTime DateRedaction { get; set; }
    public string? Observations { get; set; }
    public string? SignePar { get; set; }
    public DateTime? DateSigne { get; set; }
    public string Statut { get; set; } = "brouillon";

    public Epreuve Epreuve { get; set; } = null!;
    public ICollection<CasFraude> CasFraude { get; set; } = [];
}

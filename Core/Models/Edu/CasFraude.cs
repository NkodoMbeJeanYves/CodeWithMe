namespace CodeWithMe.Core.Models.Edu;

public class CasFraude : BaseEntity
{
    public Guid PVExamenId { get; set; }
    public Guid ApprenantId { get; set; }
    public string Type { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Sanction { get; set; }

    public PVExamen PVExamen { get; set; } = null!;
    public Apprenant Apprenant { get; set; } = null!;
}

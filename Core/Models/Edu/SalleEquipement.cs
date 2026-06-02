namespace CodeWithMe.Core.Models.Edu;

public class SalleEquipement
{
    public int Id { get; set; }
    public Guid SalleId { get; set; }
    public string Nom { get; set; } = null!;
    public string? Description { get; set; }

    public Salle Salle { get; set; } = null!;
}

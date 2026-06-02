namespace CodeWithMe.Core.Models.Edu;

public class ModeleMessage : BaseEntity
{
    public Guid TenantId { get; set; }
    public string Nom { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Evenement { get; set; } = null!;
    public string Sujet { get; set; } = null!;
    public string Corps { get; set; } = null!;
    public bool Actif { get; set; } = true;
}

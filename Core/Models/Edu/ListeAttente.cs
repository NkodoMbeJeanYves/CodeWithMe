namespace CodeWithMe.Core.Models.Edu;

public class ListeAttente : BaseEntity
{
    public Guid InscriptionId { get; set; }
    public Guid ClasseOuPromotionId { get; set; }
    public int Position { get; set; }
    public DateTime DateAjout { get; set; } = DateTime.UtcNow;
    public bool Notifie { get; set; } = false;

    public Inscription Inscription { get; set; } = null!;
}

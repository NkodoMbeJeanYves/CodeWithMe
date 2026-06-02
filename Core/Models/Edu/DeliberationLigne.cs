namespace CodeWithMe.Core.Models.Edu;

public class DeliberationLigne : BaseEntity
{
    public Guid DeliberationId { get; set; }
    public Guid ApprenantId { get; set; }
    public decimal MoyenneGenerale { get; set; }
    public decimal? EctsAcquis { get; set; }
    public bool SemestreValide { get; set; } = false;
    public string? Decision { get; set; }
    public string? Mention { get; set; }
    public string? Commentaire { get; set; }
    public bool CasSpecial { get; set; } = false;
    public bool ModifieeManuel { get; set; } = false;

    public Deliberation Deliberation { get; set; } = null!;
    public Apprenant Apprenant { get; set; } = null!;
}

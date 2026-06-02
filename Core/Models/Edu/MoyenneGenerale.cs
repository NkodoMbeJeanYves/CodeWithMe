namespace CodeWithMe.Core.Models.Edu;

public class MoyenneGenerale : BaseEntity
{
    public Guid ApprenantId { get; set; }
    public Guid? PeriodeId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public decimal Moyenne { get; set; }
    public int? Rang { get; set; }
    public int? TotalApprenants { get; set; }
    public string? Mention { get; set; }
    public decimal? EctsAcquis { get; set; }
    public decimal? EctsTotal { get; set; }
    public bool Validee { get; set; } = false;
    public string Statut { get; set; } = "en_cours";

    public Apprenant Apprenant { get; set; } = null!;
}

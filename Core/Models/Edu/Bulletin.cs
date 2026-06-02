namespace CodeWithMe.Core.Models.Edu;

public class Bulletin : BaseEntity
{
    public Guid ApprenantId { get; set; }
    public Guid EtablissementId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public Guid PeriodeId { get; set; }
    public string Type { get; set; } = "bulletin";
    public string Statut { get; set; } = "brouillon";
    public decimal? MoyenneGenerale { get; set; }
    public decimal? MoyenneClasse { get; set; }
    public int? Rang { get; set; }
    public int? TotalApprenants { get; set; }
    public string? Mention { get; set; }
    public string? Decision { get; set; }
    public string? AppreciationGenerale { get; set; }
    public string? AppreciationProfPrincipal { get; set; }
    public int BilanAbsTotal { get; set; } = 0;
    public int BilanAbsJustifiees { get; set; } = 0;
    public int BilanAbsInjustifiees { get; set; } = 0;
    public string? SignePar { get; set; }
    public DateTime? DateSigne { get; set; }
    public DateTime? DatePublication { get; set; }
    public string? PdfUrl { get; set; }

    public Apprenant Apprenant { get; set; } = null!;
    public PeriodeEdu Periode { get; set; } = null!;
    public ICollection<BulletinLigne> Lignes { get; set; } = [];
}

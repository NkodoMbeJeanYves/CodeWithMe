namespace CodeWithMe.Core.Models.Edu;

public class AffectationMatiere : BaseEntity
{
    public Guid EnseignantId { get; set; }
    public Guid MatiereId { get; set; }
    public Guid? ClasseId { get; set; }
    public Guid? PromotionId { get; set; }
    public Guid AnneeAcademiqueId { get; set; }
    public int HeuresPrevues { get; set; }
    public int HeuresRealisees { get; set; } = 0;
    public bool Actif { get; set; } = true;

    public Enseignant Enseignant { get; set; } = null!;
    public Matiere Matiere { get; set; } = null!;
}

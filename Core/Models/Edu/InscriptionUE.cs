namespace CodeWithMe.Core.Models.Edu;

public class InscriptionUE : BaseEntity
{
    public Guid InscriptionId { get; set; }
    public Guid UeId { get; set; }
    public string Statut { get; set; } = "inscrit";

    public Inscription Inscription { get; set; } = null!;
    public UniteEnseignement Ue { get; set; } = null!;
}

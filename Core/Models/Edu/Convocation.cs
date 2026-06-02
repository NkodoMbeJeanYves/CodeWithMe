namespace CodeWithMe.Core.Models.Edu;

public class Convocation : BaseEntity
{
    public Guid EpreuveId { get; set; }
    public Guid ApprenantId { get; set; }
    public string? NumeroPlace { get; set; }
    public string? Salle { get; set; }
    public string Statut { get; set; } = "generee";
    public DateTime? DateEnvoi { get; set; }
    public bool Eligible { get; set; } = true;
    public string? MotifIneligibilite { get; set; }

    public Epreuve Epreuve { get; set; } = null!;
    public Apprenant Apprenant { get; set; } = null!;
}

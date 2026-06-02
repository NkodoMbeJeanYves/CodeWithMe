namespace CodeWithMe.Core.Models.Edu;

public class InscriptionGroupe
{
    public Guid InscriptionId { get; set; }
    public Guid GroupeId { get; set; }

    public Inscription Inscription { get; set; } = null!;
    public Groupe Groupe { get; set; } = null!;
}

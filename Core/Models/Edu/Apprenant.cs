namespace CodeWithMe.Core.Models.Edu;

public class Apprenant : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public string NumeroInscription { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Nom { get; set; } = null!;
    public string Prenom { get; set; } = null!;
    public DateOnly DateNaissance { get; set; }
    public string LieuNaissance { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public string Nationalite { get; set; } = null!;
    public string? PhotoUrl { get; set; }
    public string Adresse { get; set; } = null!;
    public string Ville { get; set; } = null!;
    public string Pays { get; set; } = null!;
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public string Statut { get; set; } = "actif";

    public ICollection<Tuteur> Tuteurs { get; set; } = [];
    public ICollection<PieceJustificative> PiecesJustificatives { get; set; } = [];
    public ICollection<Inscription> Inscriptions { get; set; } = [];
    public ICollection<NoteEdu> Notes { get; set; } = [];
    public ICollection<Absence> Absences { get; set; } = [];
    public ICollection<Presence> Presences { get; set; } = [];
    public ICollection<Convocation> Convocations { get; set; } = [];
    public ICollection<Bulletin> Bulletins { get; set; } = [];
    public ICollection<MoyenneGenerale> MoyennesGenerales { get; set; } = [];
}

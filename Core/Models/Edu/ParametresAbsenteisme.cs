namespace CodeWithMe.Core.Models.Edu;

public class ParametresAbsenteisme : BaseEntity
{
    public Guid EtablissementId { get; set; }
    public int SeuilAlerte { get; set; }
    public int DelaiSaisieHeures { get; set; }
    public bool AbsenceExamenNote0 { get; set; } = true;
    public bool NotificationParent { get; set; } = true;
    public int NotificationDelaiHeures { get; set; } = 24;
}

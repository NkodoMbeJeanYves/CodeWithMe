using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[NotMapped]
public class DaySettings
{
    public string Language { get; set; } = "EN";
    public Dictionary<int, Dictionary<string, string>> Days { get; set; }
    public DaySettings()
    {
        Days = new Dictionary<int, Dictionary<string, string>> { };
    }
}


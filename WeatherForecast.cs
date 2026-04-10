using System.ComponentModel.DataAnnotations;

namespace CodeWithMe
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "TempC is required")]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [Required(ErrorMessage = "Summary is required")]
        public string? Summary { get; set; }
    }
}

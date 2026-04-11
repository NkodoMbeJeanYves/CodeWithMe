using System.ComponentModel.DataAnnotations;

namespace CodeWithMe
{
    public record class GameDto(
        int Id, string Name, string Description, string Genre, DateOnly ReleaseDate, decimal Price
    );

    public record GameCreateDto(
        [Required]
        [StringLength(20, ErrorMessage = "Name field must be lower than 20 characters", MinimumLength = 5)]
        string Name,

        [Required]
        [StringLength(20, ErrorMessage = "Description field must be lower than 20 characters", MinimumLength = 5)]
        string Description,

        [Required]
        [StringLength(20, ErrorMessage = "Genre field must be lower than 20 characters", MinimumLength = 5)]
        string Genre,

        [Required]
        [DataType(DataType.Date, ErrorMessage = "The input must be a correct date format (yyyy-mm-dd)")]
        DateOnly ReleaseDate,

        decimal Price
    );

}

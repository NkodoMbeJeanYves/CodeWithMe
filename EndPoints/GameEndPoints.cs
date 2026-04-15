using CodeWithMe.Core.Services;
using System.ComponentModel.DataAnnotations;


namespace CodeWithMe.EndPoints
{
    public static class GameEndPoints
    {
        const string GAME_END_POINT = "api/games";
        private static readonly List<GameDto> _games = [
            new GameDto(1, "street fighter", "A fighting game developed by Capcom.", "Fighting", new DateOnly(1987, 8, 30), 19.99m),
            new GameDto(2, "mortal kombat", "A fighting game developed by Midway Games.", "Fighting", new DateOnly(1992, 10, 8), 29.99m),
            new GameDto(3, "super mario bros", "A platform game developed by Nintendo.", "Platformer", new DateOnly(1985, 9, 13), 49.99m),
            new GameDto(4, "the legend of zelda", "An action-adventure game developed by Nintendo.", "Action-Adventure", new DateOnly(1986, 2, 21), 59.99m),
            new GameDto(5, "final fantasy", "A role-playing game developed by Square Enix.", "Role-Playing", new DateOnly(1987, 12, 18), 39.99m)
        ];
        public static void MapGameEndPoints(this WebApplication app)
        {
            var group = app.MapGroup(GAME_END_POINT).WithTags("Games");
            group.MapGet("/", (FakeService service) => service.GetMessage());

            group.MapGet("/{id}", (int id) =>
            {
                var game = _games.FirstOrDefault(g => g.Id == id);
                return game is not null ? Results.Ok(game) : Results.NotFound();
            }).WithName("GetGameById");

            group.MapPost("/", (GameCreateDto game) =>
            {
                //  uncomment if you haven't enable validation service in program.cs
                //  Manual Validation

                //if (string.IsNullOrEmpty(game.name.Trim()) || string.IsNullOrEmpty(game.description.Trim()) || string.IsNullOrEmpty(game.genre.Trim()))
                //{
                //    return Results.BadRequest("Name, description, and genre are required fields.");
                //}

                var newId = _games.Max(g => g.Id) + 1;
                _games.Add(new GameDto(newId, game.Name, game.Description, game.Genre, game.ReleaseDate, game.Price));
                return Results.CreatedAtRoute("GetGameById", new { id = newId }, game);
            });

            group.MapPut("/{id}", (int id, GameCreateDto updatedGame) =>
            {
                var context = new ValidationContext(updatedGame);
                var results = new List<ValidationResult>();
                Console.WriteLine($"Validating game with Name: {updatedGame.Name}, Description: {updatedGame.Description}, Genre: {updatedGame.Genre}, ReleaseDate: {updatedGame.ReleaseDate}, Price: {updatedGame.Price}");
                //if (!Validator.TryValidateObject(updatedGame, context, results, validateAllProperties: true))
                //{
                //    var errors = results.ToDictionary(r => r.MemberNames.FirstOrDefault() ?? "General", r => r.ErrorMessage);
                //    return Results.BadRequest(errors);
                //}

                var index = _games.FindIndex(g => g.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }
                _games.RemoveAt(index);
                _games.Add(new GameDto(id, updatedGame.Name, updatedGame.Description, updatedGame.Genre, updatedGame.ReleaseDate, updatedGame.Price));
                return Results.NoContent();
            });

            group.MapDelete("/{id}", (int id) =>
            {
                var game = _games.FirstOrDefault(g => g.Id == id);
                if (game is null)
                {
                    return Results.NotFound();
                }
                _games.RemoveAll(game => game.Id == id);
                return Results.NoContent();
            });
        }
    }
}


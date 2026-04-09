using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Core
{
    public static class DataExtensions
    {
        public static void MigrateDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiContext>();
            dbContext.Database.Migrate();
        }
    }
}

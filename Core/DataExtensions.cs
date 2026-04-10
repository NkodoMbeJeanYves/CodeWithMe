using CodeWithMe.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Core
{
    public static class DataExtensions
    {
        /*
         * this method extends the WebApplication class to include a method for migrating the database. 
         * It creates a scope for the application's services, retrieves the ApiContext from the service provider, 
         * and calls the Migrate method on the database to apply any pending migrations. 
         * This ensures that the database schema is up to date with the application's data model when the application starts.
         */
        public static void MigrateDb(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApiContext>();
                dbContext.Database.Migrate();   // ensures DB is up-to-date, executing migration

                if (!dbContext.Set<Subject>().Any())
                {
                    dbContext.Set<Subject>().AddRange(
                        new Subject
                        {
                            SubjectId = "math",
                            SubjectName = "Mathematics",
                            Description = "The study of numbers, shapes, and patterns."
                        },
                        new Subject
                        {
                            SubjectId = "physics",
                            SubjectName = "Physics",
                            Description = "The study of matter, energy, and the fundamental forces of nature."
                        },
                        new Subject
                        {
                            SubjectId = "chemistry",
                            SubjectName = "Chemistry",
                            Description = "The study of substances, their properties, and how they interact with each other."
                        }
                    );
                    dbContext.SaveChanges();
                }

            }

        }

        public static void establishConnection(this WebApplicationBuilder builder)
        {
            // Add database context before building the app
            // ref: https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql#readme-body-tab
            var connectionString = builder.Configuration.GetConnectionString("ApiConnection");
            var serverVersion = new MySqlServerVersion(new Version(8, 4, 3));

            /**
             * ApiContext has a scoped service lifetime because:
             * 1. It ensures that a new instance of ApiContext is created per request, which is important for managing database connections and ensuring thread safety.
             * 2. It allows for better performance and resource management by reusing the same instance of ApiContext within a single request, while still providing isolation between different requests.
             * 3. It ensures that the database context is properly disposed of at the end of each request, preventing potential memory leaks and ensuring that database connections are released back to the connection pool in a timely manner.
             * 4. ApiContext is not thread-safe. Scope avoids to concurrency issues that could arise if multiple requests were to share the same instance of ApiContext.
             * 5. Makes it easier to manage transactions and ensure data consistency.
             */
            builder.Services.AddDbContext<ApiContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, serverVersion)
                    // The following three options help with debugging, but should
                    // be changed or removed for production.
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
        }
    }
}

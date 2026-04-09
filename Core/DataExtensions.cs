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
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiContext>();
            dbContext.Database.Migrate();
        }

        public static void seedSubjects(this WebApplicationBuilder builder)
        {
            // Add database context before building the app
            // ref: https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql#readme-body-tab
            var connectionString = builder.Configuration.GetConnectionString("ApiConnection");
            var serverVersion = new MySqlServerVersion(new Version(8, 4, 3));
            // Replace 'YourDbContext' with the name of your own DbContext derived class.
            builder.Services.AddDbContext<ApiContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, serverVersion)
                    // The following three options help with debugging, but should
                    // be changed or removed for production.
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .UseSeeding((context, _) => {
                        Console.WriteLine("Seeding Subjects..." + context.Set<Subject>().Any(), !context.Set<Subject>().Any());
                        if (context.Set<Subject>().Any())
                        {
                            context.Set<Subject>().AddRange(
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
                            context.SaveChanges();
                        }
                    })

            );
        }
    }
}

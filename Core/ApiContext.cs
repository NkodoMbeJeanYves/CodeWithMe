using CodeWithMe.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace CodeWithMe.Core
{
    public class ApiContext(DbContextOptions<ApiContext> options): DbContext(options)
    {
        public DbSet<Subject> Subjects { get; set; }
    }
}

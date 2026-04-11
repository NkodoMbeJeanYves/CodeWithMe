using CodeWithMe.Core;
using CodeWithMe.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.EndPoints
{
    public static class SubjectEndPoints
    {
        const string SUBJECT_END_POINT = "/subjects";

        public static void MapSubjectEndPoints(this WebApplication app)
        {
            var group = app.MapGroup(SUBJECT_END_POINT).WithTags("Subjects");
            group.MapGet("/", async (ApiContext db) => await db.Subjects.Select(subject => new SubjectDto(
                subject.SubjectId,
                subject.SubjectName,
                subject.Description
                ))
            .AsNoTracking() // no tracking since we are only reading data
            .ToListAsync());

            group.MapGet("/{id}", async (string id, ApiContext db) =>
            {
                var subject = await db.Subjects.FindAsync(id);
                return subject is not null ? Results.Ok(subject) : Results.NotFound();
            }).WithName("GetSubjectById");

            group.MapPost("/", async (SubjectDto subject, ApiContext db) =>
            {
                if (string.IsNullOrEmpty(subject.SubjectId.Trim()) || string.IsNullOrEmpty(subject.SubjectName.Trim()) || string.IsNullOrEmpty(subject?.Description?.Trim()))
                {
                    return Results.BadRequest("Name, description, and Id are required fields.");
                }
                var newSubject = new Subject()
                {
                    SubjectId = subject.SubjectId,
                    SubjectName = subject.SubjectName,
                    Description = subject.Description,
                };
                newSubject.CreatedAt = DateTime.UtcNow;
                newSubject.UpdatedAt = DateTime.UtcNow;
                db.Subjects.Add(newSubject);
                await db.SaveChangesAsync();
                return Results.CreatedAtRoute("GetSubjectById", new { id = newSubject.SubjectId }, newSubject);
            });

            group.MapPut("/{id}", async (string id, SubjectDto updatedSubject, ApiContext db) =>
            {
                var subject = await db.Subjects.FindAsync(id);
                if (subject is null) return Results.NotFound();
                subject.SubjectName = updatedSubject.SubjectName;
                subject.Description = updatedSubject.Description;
                subject.UpdatedAt = DateTime.UtcNow;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            group.MapDelete("/{id}", async (string id, ApiContext db) =>
            {
                var subject = await db.Subjects.FindAsync(id);
                if (subject is null) return Results.NotFound();
                db.Subjects.Remove(subject);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}

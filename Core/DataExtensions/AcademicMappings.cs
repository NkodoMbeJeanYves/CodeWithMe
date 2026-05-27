using CodeWithMe.Core.Dtos.Attendance;
using CodeWithMe.Core.Dtos.Classe;
using CodeWithMe.Core.Dtos.Filiere;
using CodeWithMe.Core.Dtos.Grade;
using CodeWithMe.Core.Dtos.Room;
using CodeWithMe.Core.Dtos.Session;
using CodeWithMe.Core.Dtos.Student;
using CodeWithMe.Core.Dtos.Teacher;
using CodeWithMe.Core.Models;
using AttendanceModel = CodeWithMe.Core.Models.Attendance;
using ClasseModel = CodeWithMe.Core.Models.Classe;
using FiliereModel = CodeWithMe.Core.Models.Filiere;
using GradeModel = CodeWithMe.Core.Models.Grade;
using RoomModel = CodeWithMe.Core.Models.Room;
using SessionModel = CodeWithMe.Core.Models.CourseSession;
using StudentModel = CodeWithMe.Core.Models.Student;
using TeacherModel = CodeWithMe.Core.Models.Teacher;

namespace CodeWithMe.Core.DataExtensions;

/// <summary>
/// Mappings DTO ↔ entité pour le domaine academic.
/// Regroupé hors de <see cref="DtoExtension"/> pour ne pas en faire un fichier monstre.
/// </summary>
public static class AcademicMappings
{
    public static FiliereDto ToDto(this FiliereModel f) => new(f.Code, f.Name, f.Id, f.TenantId);
    public static FiliereModel ToEntity(this FiliereCreateDto dto) => new()
    {
        Id = Guid.NewGuid().ToString(),
        Code = dto.Code,
        Name = dto.Name
    };

    public static ClasseDto ToDto(this ClasseModel c) =>
        new(c.Code, c.Name, c.Level, c.FiliereId, c.AcademicYear, c.Id, c.TenantId);
    public static ClasseModel ToEntity(this ClasseCreateDto dto) => new()
    {
        Id = Guid.NewGuid().ToString(),
        Code = dto.Code,
        Name = dto.Name,
        Level = dto.Level,
        FiliereId = dto.FiliereId,
        AcademicYear = dto.AcademicYear
    };

    public static StudentDto ToDto(this StudentModel s) => new(
        s.UserId, s.Matricule, s.ClasseId, s.FiliereId, s.DateOfBirth, s.Gender,
        s.EnrolledAt, s.Status, s.Id, s.TenantId);
    public static StudentModel ToEntity(this StudentCreateDto dto) => new()
    {
        Id = Guid.NewGuid().ToString(),
        UserId = dto.UserId,
        Matricule = dto.Matricule,
        ClasseId = dto.ClasseId,
        FiliereId = dto.FiliereId,
        DateOfBirth = dto.DateOfBirth,
        Gender = dto.Gender,
        EnrolledAt = dto.EnrolledAt ?? DateTime.UtcNow,
        Status = dto.Status
    };

    public static TeacherDto ToDto(this TeacherModel t) => new(
        t.UserId, t.Matricule, t.HireDate, t.ContractType, t.WeeklyHours, t.Status, t.Id, t.TenantId);
    public static TeacherModel ToEntity(this TeacherCreateDto dto) => new()
    {
        Id = Guid.NewGuid().ToString(),
        UserId = dto.UserId,
        Matricule = dto.Matricule,
        HireDate = dto.HireDate,
        ContractType = dto.ContractType,
        WeeklyHours = dto.WeeklyHours,
        Status = dto.Status
    };

    public static RoomDto ToDto(this RoomModel r) => new(r.Code, r.Name, r.Capacity, r.Id, r.TenantId);
    public static RoomModel ToEntity(this RoomCreateDto dto) => new()
    {
        Id = Guid.NewGuid().ToString(),
        Code = dto.Code,
        Name = dto.Name,
        Capacity = dto.Capacity
    };

    public static SessionDto ToDto(this SessionModel s) => new(
        s.SubjectId, s.TeacherId, s.ClasseId, s.RoomId, s.StartAt, s.EndAt, s.Type, s.Status,
        new SessionAttendanceSummary(s.AttendanceRecorded, s.PresentCount, s.AbsentCount),
        s.Notes, s.Id, s.TenantId);
    public static SessionModel ToEntity(this SessionCreateDto dto) => new()
    {
        Id = Guid.NewGuid().ToString(),
        SubjectId = dto.SubjectId,
        TeacherId = dto.TeacherId,
        ClasseId = dto.ClasseId,
        RoomId = dto.RoomId,
        StartAt = dto.StartAt,
        EndAt = dto.EndAt,
        Type = dto.Type,
        Notes = dto.Notes
    };

    public static AttendanceDto ToDto(this AttendanceModel a) => new(
        a.SessionId, a.StudentId, a.Status, a.MinutesLate, a.Justification, a.DocumentId, a.Id, a.TenantId);

    public static GradeDto ToDto(this GradeModel g) => new(
        g.StudentId, g.SubjectId, g.TeacherId, g.SessionId, g.Value, g.Scale, g.Coefficient,
        g.Type, g.Period, g.Comment, g.ValidatedBy, g.PublishedAt, g.Id, g.TenantId);
    public static GradeModel ToEntity(this GradeCreateDto dto) => new()
    {
        Id = Guid.NewGuid().ToString(),
        StudentId = dto.StudentId,
        SubjectId = dto.SubjectId,
        TeacherId = dto.TeacherId,
        SessionId = dto.SessionId,
        Value = dto.Value,
        Scale = dto.Scale,
        Coefficient = dto.Coefficient,
        Type = dto.Type,
        Period = dto.Period,
        Comment = dto.Comment
    };
}

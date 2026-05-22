using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Subject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[Route("api/subjects")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly ILogger<ProgramController> _logger;

    public SubjectController(ILogger<ProgramController> logger, ApiContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        _logger.LogDebug("Getting subjects with page number {PageNumber} and page size {PageSize}", pageNumber, pageSize);
        var subjects = await _context.Subjects.AsNoTracking().ToPagedResultAsync(pageNumber, pageSize);
        return Ok(subjects);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDto>> GetSubjectById(string id)
    {
        _logger.LogDebug("Getting subject with ID {SubjectId}", id);
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null)
        {
            _logger.LogWarning("Subject with ID {SubjectId} not found", id);
            return NotFound();
        }
        return Ok(subject.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<SubjectDto>> Store(SubjectDto dto)
    {
        _logger.LogDebug("Creating a new subject with name {SubjectName}", dto.SubjectName);
        var subject = dto.ToEntity();
        subject.SubjectId = Guid.NewGuid().ToString();
        _logger.LogDebug("Generated new subject ID {SubjectId} for subject name {SubjectName}", subject.SubjectId, subject.SubjectName);
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Subject with ID {SubjectId} created successfully", subject.SubjectId);
        return new ActionResult<SubjectDto>(subject.ToDto());
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<SubjectDto>> Update([FromRoute] string id, [FromBody] SubjectDto dto)
    {
        _logger.LogDebug("Updating subject with ID {SubjectId}", id);
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null)
        {
            _logger.LogWarning("Subject with ID {SubjectId} not found for update", id);
            return NotFound();
        }
        subject.SubjectName = dto.SubjectName;
        subject.Description = dto.Description;
        _context.Subjects.Update(subject);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Subject with ID {SubjectId} updated successfully", subject.SubjectId);
        return Ok(subject.ToDto());
    }
}

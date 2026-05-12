using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Program;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeWithMe.Controllers;

[Route("api/programs")]
[ApiController]
[Authorize]
public class ProgramController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly ILogger<ProgramController> _logger;

    public ProgramController(ILogger<ProgramController> logger, ApiContext context)
    {
        _logger = logger;
        _context = context;
    }

    // GET api/programs/school/{school_id}
    [HttpGet("/school/{schoolId}")]
    public async Task<ActionResult<IQueryable<ProgramDto>>> Index([FromRoute] string schoolId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            _logger.LogDebug("Fetching programs for school {SchoolId} with pagination. PageNumber: {PageNumber}, PageSize: {PageSize}", schoolId, pageNumber, pageSize);
            var result = await _context.Programs
                .Where(p => p.SchoolId == schoolId)
                .Select(it => it.ToDto())
                .AsNoTracking()
                .ToPagedResultAsync(pageNumber, pageSize);

            _logger.LogInformation("Successfully retrieved {RecordCount} programs from page {PageNumber}",
                result.Data.Count(), pageNumber);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching programs. PageNumber: {PageNumber}, PageSize: {PageSize}",
                pageNumber, pageSize);
            // Return a 500 Internal Server Error with a message
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }

    // Get api/programs/{program_id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProgramDto>> GetProgramById(string id)
    {
        try
        {
            _logger.LogDebug("Attempting to retrieve program with ID: {ProgramId}", id);

            var program = await _context.Programs.FindAsync(id);

            if (program is null)
            {
                _logger.LogWarning("Program not found. ProgramId: {ProgramId}", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved program. ProgramId: {ProgramId}, ProgramName: {ProgramName}",
                id, program.Name);

            return Ok(program.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching program. ProgramId: {ProgramId}", id);
            // Return a 500 Internal Server Error with a message
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }


    [HttpPost]
    public async Task<ActionResult<ProgramDto>> Store(ProgramDto dto)
    {
        try
        {
            _logger.LogDebug("Attempting to create new program. ProgramName: {ProgramName}", dto.Name);

            var program = dto.ToEntity();
            program.Program_id = Guid.NewGuid().ToString();

            _logger.LogDebug("Generated new ProgramId: {ProgramId}", program.Program_id);

            _context.Programs.Add(program);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully created new program. ProgramId: {ProgramId}, ProgramName: {ProgramName}",
                program.Program_id, program.Name);

            return new ActionResult<ProgramDto>(program.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while storing new program. ProgramName: {ProgramName}", dto.Name);
            // Return a 500 Internal Server Error with a message
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<ProgramDto>> update(string id, [FromBody] ProgramDto dto)
    {
        try
        {
            _logger.LogDebug("Attempting to update program. ProgramId: {ProgramId}, NewName: {NewName}", id, dto.Name);

            var program = await _context.Programs.FindAsync(id);
            if (program is null)
            {
                _logger.LogWarning("Program not found for update. ProgramId: {ProgramId}", id);
                return NotFound();
            }

            var oldName = program.Name;
            program.Name = dto.Name;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully updated program. ProgramId: {ProgramId}, OldName: {OldName}, NewName: {NewName}",
                id, oldName, program.Name);

            return Ok(program.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating program. ProgramId: {ProgramId}, NewName: {NewName}",
                id, dto.Name);
            // Return a 500 Internal Server Error with a message
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<ProgramDto>> Delete(string id)
    {
        try
        {
            _logger.LogDebug("Attempting to delete program. ProgramId: {ProgramId}", id);

            var program = await _context.Programs.FindAsync(id);
            if (program is null)
            {
                _logger.LogWarning("Program not found for deletion. ProgramId: {ProgramId}", id);
                return NotFound();
            }

            _context.Programs.Remove(program);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted program. ProgramId: {ProgramId}, ProgramName: {ProgramName}",
                id, program.Name);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting program. ProgramId: {ProgramId}", id);
            // Return a 500 Internal Server Error with a message
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }
}

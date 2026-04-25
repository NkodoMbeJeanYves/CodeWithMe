using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Period;
using CodeWithMe.Core.Dtos.School;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeWithMe.Controllers;

[Route("api/periods")]
[ApiController]
public class PeriodController : ControllerBase
{
    private readonly ILogger<PeriodController> _logger;
    private readonly ApiContext _context;

    public PeriodController(ILogger<PeriodController> logger, ApiContext ctx)
    {
        _logger = logger;
        _context = ctx;
    }
    // GET: api/<PeriodController>
    [HttpGet("/{schoolId}/school")]
    public async Task<ActionResult<IEnumerable<PeriodDto>>> Index([FromRoute] string schoolId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            return Ok(await _context.Periods
                .Where(p => p.SchoolId == schoolId)
                .Select(it => it.ToDto())
                .AsNoTracking()
                .ToPagedResultAsync(pageNumber, pageSize));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching periods data");
            // Return a 500 Internal Server Error with a message
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }

    }

    // GET api/periods/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PeriodDto>> Get(string id)
    {
        try
        {
            var period = await _context.Periods.FindAsync(id);
            return period is null ? NotFound() : Ok(period.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching period with ID {id}", id);
            // Return a 500 Internal Server Error with a message
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }

    // POST api/periods
    [HttpPost("create")]
    public async Task<ActionResult<PeriodDto>> store(PeriodDto dto)
    {
        try
        {
            var period = dto.ToEntity();
            period.PeriodId = Guid.NewGuid().ToString();

            _context.Periods.Add(period);
            await _context.SaveChangesAsync();
            return new ActionResult<PeriodDto>(period.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while storing new period");
            // Return a 500 Internal Server Error with a message
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }


    [HttpPatch("{id}")]
    public async Task<ActionResult<PeriodDto>> update(string id, [FromBody] PeriodDto dto)
    {
        if (id != dto.PeriodId) return BadRequest();
        var period = dto.ToEntity();
        _context.Entry(period).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Get), new { id = period.PeriodId });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SchoolDto>> Delete(string id)
    {
        var period = await _context.Periods.FindAsync(id);
        if (period is null) return NotFound();
        _context.Periods.Remove(period);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}

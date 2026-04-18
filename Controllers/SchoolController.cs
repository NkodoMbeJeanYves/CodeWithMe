using CodeWithMe.Controllers;
using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.School;
using CodeWithMe.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    [Route("api/schools")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ApiContext _context;
        public SchoolController(ILogger<WeatherForecastController> logger, ApiContext ctx)
        {
            _context = ctx;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<School>>> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _context.Schools.OrderBy(s => s.Name)
                    .AsNoTracking().ToPagedResultAsync(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching schools data");
                // Return a 500 Internal Server Error with a message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<SchoolDto>> Get(string id)
        {
            try
            {
                var school = await _context.Schools.FindAsync(id);
                return school is null ? NotFound() : Ok(school);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching school with ID {id}", id);
                // Return a 500 Internal Server Error with a message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<SchoolDto>> store(SchoolDto dto)
        {
            try
            {
                var school = dto.ToEntity();
                school.SchoolId = Guid.NewGuid().ToString();
                // TODO 
                // Add A service to check the period params (breaks)
                _context.Schools.Add(school);
                await _context.SaveChangesAsync();
                return new ActionResult<SchoolDto>(school.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while storing new school");
                // Return a 500 Internal Server Error with a message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }
        }


        [HttpPatch("id")]
        public async Task<ActionResult<SchoolDto>> update(string id, SchoolDto dto)
        {
            if (id != dto.SchoolId) return BadRequest();
            var school = dto.ToEntity();
            _context.Entry(school).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Get), new { id = school.SchoolId });
        }


        [HttpDelete("id")]
        public async Task<ActionResult<SchoolDto>> Delete(string id)
        {
            var school = await _context.Schools.FindAsync(id);
            if (school is null) return NotFound();
            _context.Schools.Remove(school);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

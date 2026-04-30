using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.School;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace MyApp.Namespace
{
    [Route("api/schools")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ILogger<SchoolController> _logger;
        private readonly ApiContext _context;
        public SchoolController(ILogger<SchoolController> logger, ApiContext ctx)
        {
            _context = ctx;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolDto>>> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _context.Schools.OrderBy(s => s.Name)
                    .Select(s => s.ToDto())
                    .AsNoTracking()
                    .ToPagedResultAsync(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching schools data");
                // Return a 500 Internal Server Error with a message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolDto>> Get(string id)
        {
            try
            {
                var school = await _context.Schools
                    .Include(s => s.Periods)
                    .Select(s => s.ToDto())
                    .FirstOrDefaultAsync(s => s.SchoolId == id); ;
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
        public async Task<ActionResult<SchoolDto>> Create(SchoolDto dto)
        {
            try
            {
                var schoolDto = dto with { SchoolId = Guid.NewGuid().ToString() };
                var school = schoolDto.ToEntity();

                // Add A service to check the period params (breaks)
                school.Periods.AddRange<Period>(PeriodService.generatePeriods(dto, _logger).Select(p => p.ToEntity()).ToList());
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


        [HttpPatch("{id}")]
        public async Task<ActionResult<SchoolDto>> update(string id, [FromBody] SchoolUpdateDto dto)
        {
            // get school by id
            var existingSchool = await _context.Schools.FindAsync(id);
            if (existingSchool is null) return NotFound();

            existingSchool.Name = dto.Name; // EF compares old vs new values and only updates if there are changes, so we can set the properties directly
            existingSchool.Description = dto.Description;
            existingSchool.SchoolType = dto.SchoolType;

            await _context.SaveChangesAsync();
            return new ActionResult<SchoolDto>(existingSchool.ToDto());
        }


        [HttpDelete("{id}")]
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

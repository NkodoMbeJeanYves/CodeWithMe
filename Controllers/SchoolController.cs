using CodeWithMe.Controllers;
using CodeWithMe.Core;
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
        public async Task<ActionResult<IEnumerable<School>>> Index()
        {
            return await _context.Schools.AsNoTracking().ToListAsync();
        }

        [HttpGet("id")]
        public async Task<ActionResult<SchoolDto>> Get(string id)
        {
            var school = await _context.Schools.FindAsync(id);
            return school is null ? NotFound() : Ok(school);
        }

        [HttpPost]
        public async Task<ActionResult<SchoolDto>> store(SchoolDto dto)
        {
            var school = dto.ToEntity();
            _context.Schools.Add(school);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Get), new { id = school.SchoolId });
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

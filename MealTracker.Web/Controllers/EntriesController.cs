using System;
using System.Linq;
using System.Threading.Tasks;
using MealTracker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealTracker.Web.Controllers
{
    [Route("api/{controller}")]
    public class EntriesController : ControllerBase
    {
        private readonly MealTrackerDbContext _context;

        public EntriesController(MealTrackerDbContext context)
        {
            _context = context;
        }

        // POST api/entries
        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromBody]CreateModel model)
        {
            var meal = model.MapToMealEntry();

            _context.Add(meal);

            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetById", new {id = meal.Id}, meal);
        }

        // GET api/entries/1
        [HttpGet("{id:int}"), ActionName("GetById")]
        public async Task<IActionResult> Get(int id)
        {
            var entry = await _context.MealEntries.FindAsync(id);

            if (entry == null) return NotFound();

            return Ok(entry);
        }

        // GET api/entries
        [HttpGet("")]
        public async Task<IActionResult> Get(DateTime? date)
        {
            var query = _context.MealEntries.AsQueryable();
            
            if (date != null)
            {
                query = query.Where(e => e.Date >= date.Value && e.Date.Date < date.Value.AddDays(1).Date);
            }
            
            var entries = await query.ToListAsync();

            return Ok(entries);
        }
    }
}
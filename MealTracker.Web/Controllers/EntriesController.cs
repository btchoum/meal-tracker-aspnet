using System.Threading.Tasks;
using MealTracker.Web.Models;
using Microsoft.AspNetCore.Mvc;

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

        // POST
        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromBody]CreateModel model)
        {
            var meal = model.MapToMealEntry();

            _context.Add(meal);

            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetById", new {id = meal.Id}, meal);
        }

        [HttpGet("{id:int}"), ActionName("GetById")]
        public async Task<IActionResult> Get(int id)
        {
            var entry = await _context.MealEntries.FindAsync(id);

            if (entry == null) return NotFound();

            return Ok(entry);
        }
    }
}
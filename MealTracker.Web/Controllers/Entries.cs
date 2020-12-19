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

        // POST
        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromBody]CreateModel model)
        {
            var meal = new MealEntry
            {
                Type = model.Type,
                Calories = model.Calories,
                Carbs = model.Calories,
                Comments = model.Comments,
                Fats = model.Fats,
                Proteins = model.Proteins
            };

            _context.Add(meal);

            await _context.SaveChangesAsync();
            return Created($"api/entries/{meal.Id}", meal);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var entry = await _context.MealEntries.FindAsync(id);

            if (entry == null) return NotFound();

            return Ok(entry);
        }
    }
}
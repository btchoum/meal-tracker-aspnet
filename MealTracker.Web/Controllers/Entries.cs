using MealTracker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MealTracker.Web.Controllers
{
    [Route("api/{controller}")]
    public class Entries : ControllerBase
    {
        // POST
        //TODO: Make this async
        [HttpPost("")]
        public IActionResult PostAsync(CreateModel model)
        {
            return Ok();
        }
    }
}
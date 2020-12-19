using System.Net.Http.Json;
using System.Threading.Tasks;
using MealTracker.Web;
using MealTracker.Web.Controllers;
using MealTracker.Web.Models;
using Xunit;

namespace MealTracker.IntegrationTests
{
    public class EntryApiTests: IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public EntryApiTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [Fact]
        public async Task Post_New_Entry_Is_Successful()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var newEntry = GivenValidCreateModel();
            var response = await client.PostAsJsonAsync("api/entries", newEntry);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        private static CreateModel GivenValidCreateModel()
        {
            return new CreateModel
            {
                Type = "Breakfast",
                Proteins = 1,
                Carbs = 1,
                Fats = 1,
                Calories = 1,
                Comments = "Some comments"
            };
        }
    }
}
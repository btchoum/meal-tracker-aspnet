using System.Net.Http.Json;
using System.Threading.Tasks;
using MealTracker.Web;
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
            var newEntry = new
            {
                type = "Breakfast",
                proteins = 1,
                carbs = 1,
                fats = 1,
                calories = 1,
                comments = "Some comments"
            };
            var response = await client.PostAsJsonAsync("api/entries", newEntry);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}
using System.Net.Http.Json;
using System.Threading.Tasks;
using MealTracker.Web;
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
        public async Task Create_New_Entry_Is_Successful()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var newEntry = GivenValidCreateModel();
            var response = await client.PostAsJsonAsync("api/entries", newEntry);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
        
        [Fact]
        public async Task Create_And_Read_Round_Trip()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newEntry = GivenValidCreateModel();
            var response = await client.PostAsJsonAsync("api/entries", newEntry);
            var getUrl = response.Headers.Location?.ToString();
            Assert.NotNull(getUrl);

            // Act
            var getResponse = await client.GetAsync(getUrl);
            getResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            var createdEntry = await getResponse.Content.ReadFromJsonAsync<MealEntryDto>();
            
            // Assert
            Assert.NotNull(createdEntry);
            Assert.Equal(newEntry.Type, createdEntry.Type);
            Assert.Equal(newEntry.Calories, createdEntry.Calories);
            Assert.Equal(newEntry.Carbs, createdEntry.Carbs);
            Assert.Equal(newEntry.Proteins, createdEntry.Proteins);
            Assert.Equal(newEntry.Fats, createdEntry.Fats);
            Assert.Equal(newEntry.Comments, createdEntry.Comments);
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
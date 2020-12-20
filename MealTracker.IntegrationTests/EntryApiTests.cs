using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MealTracker.Web;
using MealTracker.Web.Models;
using Xunit;

namespace MealTracker.IntegrationTests
{
    public class EntryApiTests: IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
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
            response.EnsureSuccessStatusCode();
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
            getResponse.EnsureSuccessStatusCode();
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
        
        [Fact]
        public async Task Get_When_Id_Does_Not_Exists()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var badId = int.MaxValue;
            var response = await client.GetAsync($"api/entries/{badId}");
            
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
         }


        [Fact]
        public async Task Get_All()
        {
            // Arrange
            await CreateOneEntry();
            await CreateOneEntry();
            
            // Act
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/entries");
            response.EnsureSuccessStatusCode();
            
            // Assert
            var entries = await response.Content.ReadFromJsonAsync<IEnumerable<MealEntryDto>>();
            Assert.NotNull(entries);
            Assert.Equal(2, entries.Count());
        }
        
        [Fact]
        public async Task Get_Filters_By_Date()
        {
            // Arrange
            // Matching entries
            await CreateOneEntry(DateTime.Parse("2020-01-01"));
            await CreateOneEntry(DateTime.Parse("2020-01-01"));
            // Non matching entry
            await CreateOneEntry(DateTime.Parse("2020-01-02"));
            
            // Act
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/entries?date=2020-01-01");
            response.EnsureSuccessStatusCode();
            
            // Assert
            var entries = await response.Content.ReadFromJsonAsync<IEnumerable<MealEntryDto>>();
            Assert.NotNull(entries);
            Assert.Equal(2, entries.Count());
        }

        private async Task CreateOneEntry(DateTime? date = null)
        {
            var client = _factory.CreateClient();
            var newEntry = GivenValidCreateModel();

            if (date != null)
            {
                newEntry.Date = date;
            }
            
            await client.PostAsJsonAsync("api/entries", newEntry);
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

        public void Dispose()
        {
            _factory.CleanDatabase();
        }
    }
}
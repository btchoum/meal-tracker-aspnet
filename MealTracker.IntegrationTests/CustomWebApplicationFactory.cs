using System;
using System.Linq;
using MealTracker.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
// ReSharper disable StaticMemberInGenericType

namespace MealTracker.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private static readonly object Lock = new object();
        private static bool _databaseInitialized;

        public void CleanDatabase()
        {
            using (var scope = Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MealTrackerDbContext>();
                Utilities.InitializeDbForTests(db);
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<MealTrackerDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<MealTrackerDbContext>(options =>
                {
                    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MealTrackerTests");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    lock (Lock)
                    {
                        if (_databaseInitialized) return;
                        
                        var db = scopedServices.GetRequiredService<MealTrackerDbContext>();
                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();

                        try
                        {
                            Utilities.InitializeDbForTests(db);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred seeding the " +
                                                "database with test messages. Error: {Message}", ex.Message);
                        }
                        _databaseInitialized = true;
                    }
                }
            });
        }
    }

    public class Utilities
    {
        public static void InitializeDbForTests(MealTrackerDbContext db)
        {
            var entries = db.MealEntries.ToList();
            foreach (var entry in entries)
            {
                db.Remove(entry);
            }

            db.SaveChanges();
        }
    }
}
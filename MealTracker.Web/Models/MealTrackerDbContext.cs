using Microsoft.EntityFrameworkCore;

namespace MealTracker.Web.Models
{
    public class MealTrackerDbContext : DbContext
    {
        public MealTrackerDbContext(DbContextOptions<MealTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<MealEntry> MealEntries { get; set; }
    }
}
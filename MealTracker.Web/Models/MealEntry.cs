using System;

namespace MealTracker.Web.Models
{
    public class MealEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public int Proteins { get; set; }
        public int Carbs { get; set; }
        public int Fats { get; set; }
        public int Calories { get; set; }
        public string Comments { get; set; }
    }
}
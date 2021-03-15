using System;
using System.Linq;
using api.fruits.Models;

namespace api.fruits.Data
{
    public static class FruitContextDbInitializer
    {
        public static void Initialize(FruitContext context)
        {
            context.Database.EnsureCreated();

            // Look for any fruits.
            if (context.Fruit.Any())
            {
                return;   // DB has been seeded
            }

            var random = new Random();

            var fruits = new Fruit[]
            {
                new Fruit { Name = "Granny Smith Apple", Color = "Green", Type = "Pomme" },
                new Fruit { Name = "Red Grape", Color = "Purple", Type = "Berry" },
                new Fruit { Name = "Peach", Color = "Pink", Type = "Drupe" },
                new Fruit { Name = "Orange", Color = "Orange", Type = "Hesperidium" },
                new Fruit { Name = "Lemon", Color = "Yellow", Type = "Hesperidium" },
                new Fruit { Name = "Cantaloupe", Color = "Orange", Type = "Pepo" },
                new Fruit { Name = "Tomato", Color = "Red", Type = "Berry" },
                new Fruit { Name = "Green Olive", Color = "Green", Type = "Drupe" },
            };

            context.Fruit.AddRange(fruits);
            context.SaveChanges();
        }
    }
}
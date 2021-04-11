using System;
using System.Linq;
using api.hat.Models;

namespace api.hat.Data
{
    public static class HatContextDbInitializer
    {
        public static void Initialize(HatContext context)
        {
            context.Database.EnsureCreated();

            // Look for any hats.
            if (context.Hats.Any())
            {
                return;   // DB has been seeded
            }

            var hats = new Hat[]
            {
                new Hat { Id = Guid.NewGuid(), Name = "Fedora", Color = "Brown", Material = "Felt" },
                new Hat { Id = Guid.NewGuid(), Name = "Trilby", Color = "Gray", Material = "Felt" },
                new Hat { Id = Guid.NewGuid(), Name = "Panama Hat", Color = "White", Material = "Cloth" },
                new Hat { Id = Guid.NewGuid(), Name = "Bowler", Color = "Brown", Material = "Felt" },
                new Hat { Id = Guid.NewGuid(), Name = "Snapback", Color = "Black", Material = "Cloth" },
                new Hat { Id = Guid.NewGuid(), Name = "Dad Hat", Color = "Orange", Material = "Cloth" },
                new Hat { Id = Guid.NewGuid(), Name = "Newsboy", Color = "Black", Material = "Cloth" },
                new Hat { Id = Guid.NewGuid(), Name = "Flat Cap", Color = "Gray", Material = "Cloth" },
                new Hat { Id = Guid.NewGuid(), Name = "Beanie", Color = "Purple", Material = "Cloth" },
                new Hat { Id = Guid.NewGuid(), Name = "Bucket Hat", Color = "Black", Material = "Felt" },
                new Hat { Id = Guid.NewGuid(), Name = "Baseball Cap", Color = "Blue", Material = "Cloth" },
            };

            context.Hats.AddRange(hats);
            context.SaveChanges();
        }
    }
}
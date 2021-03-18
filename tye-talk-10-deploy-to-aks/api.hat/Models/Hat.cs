using System;

namespace api.hat.Models
{
    public class Hat
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string Material { get; set; }
    }
}
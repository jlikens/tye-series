using System;

namespace api.birds.Models
{
    public class Bird
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public double WingSpan{ get; set; }
    }
}
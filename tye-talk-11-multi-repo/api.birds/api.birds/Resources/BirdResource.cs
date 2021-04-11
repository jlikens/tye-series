using System;

namespace api.birds.Resources
{
    public class BirdResource
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public double WingSpan { get; set; }
    }
}
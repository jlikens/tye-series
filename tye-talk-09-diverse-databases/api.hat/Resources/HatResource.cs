﻿using System;

namespace api.hat.Resources
{
    public class HatResource
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string Material { get; set; }
    }
}
﻿namespace api.birds
{
    public class Config : IConfig
    {
        public bool SeedDatabase { get; set; }
        public string MongoDbConnectionString { get; set; }
    }
}
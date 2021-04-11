namespace api.fruits
{
    public class Config : IConfig
    {
        public bool RunDbMigrations { get; set; }
        public bool SeedDatabase { get; set; }
    }
}
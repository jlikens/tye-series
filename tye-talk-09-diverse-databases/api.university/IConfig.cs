namespace api.university
{
    public interface IConfig
    {
        bool RunDbMigrations { get; set; }
        bool SeedDatabase { get; set; }
    }
}
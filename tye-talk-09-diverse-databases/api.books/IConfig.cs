namespace api.books
{
    public interface IConfig
    {
        bool RunDbMigrations { get; set; }
        bool SeedDatabase { get; set; }
    }
}
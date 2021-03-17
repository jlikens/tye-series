namespace api.birds
{
    public interface IConfig
    {
        bool SeedDatabase { get; set; }
        string MongoDbConnectionString{ get; set; }
    }
}
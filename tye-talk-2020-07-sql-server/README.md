# tye-talk-2020-07-sql-server
As a quick detour, I found it pretty cool that we can use Tye to quickly drop a local, isolated SQL Server instance into our local dev environment.  To test this out, I tossed a version of the [Contoso University](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-5.0) data structures into a new microservice `api.university`, added a SQL Server 2019 container to my `tye.yaml`, used the `GetConnectionString` extension method from `Microsoft.Extensions.Configuration.Abstractions`, and away we go!  Oh, you'll also notice a new navigation entry in the `frontend` app that loads up all of the Students from the university.

**Quick Note**: If you just want to use Tye to get a SQL Server instance running, you can run `tye run tye.sqlonly.yaml`!

So how does this work?  Let's take a peek at what's changed in the `tye.yaml` since our [previous example](../tye-talk-2020-06-tye-plus-plus):

```yaml
name: tye-talk-2020-07-sql-server
...
services:
...
- name: api-university
  project: api.university/api.university.csproj
...
- name: sqlserver-usidore
  image: mcr.microsoft.com/mssql/server:2019-latest
  bindings:
  - connectionString: Server=${host},${port};Database=Usidore;User ID=sa;Password=${env:SA_PASSWORD}
    containerPort: 1433
    port: 21433 # Uncomment this line to run with a static external port, or comment it out to run with a dynamic external port
  env:
  - name: ACCEPT_EULA
    value: y
  - name: SA_PASSWORD
    value: Password1!
```

First, we added a new service entry for our University API.  As of the time of this writing, there doesn't appear to be any way to have Tye re-scan a folder structure to find newly-added projects and add them to the `tye.yaml`.  That being the case, I had to add this service manually.

Next is the bit that brings in SQL Server 2019.  Tye uses a format that is quite similar in many ways to a [Docker compose](https://docs.docker.com/compose/) file.  In this example, we're telling Tye to pull down and load up the latest version of the SQL Server 2019 Docker image, give it a specific connection string, specify the external port to map to the internal SQL Server port (1433), and give it a password for the `sa` user.

If we didn't specify the `port: 21433`, we'd get a dynamic port in much the same way that we get dynamic ports for our actual projects.  In this case, since I wanted to mess around with the SQL Server instance in [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15), I decided to force the external port to be 21433 to avoid having to re-connect to the instance every time I reran Tye.

To make things interesting, I also added in some data migrations and data seeding to show how you can use Tye plus EF migrations to quickly spool up a database with a known starting configuration.  Along with what I hope are the obvious benefits of data migrations, using seeding in this fashion is massively useful for testing weird data scenarios.  If you can be diligent about maintaining both the migrations and the seeding, you can ensure that local development always has useful, interesting data to test with that gets wiped and recreated each time you start a debugging session.

### Code Changes
The main change in this solution was the addition of `api.university`.  In `Startup.cs`, we added this code:

```csharp
namespace api.university
{
    ...
    
    public class Startup
    {
        ...
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ...
            
            TryRunMigrations(app);
            TrySeedDatabase(app);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            ...

            AddDbContexts(services);

            ...
        }

        private void AddDbContexts(IServiceCollection services)
        {
            var debugLogging = new Action<DbContextOptionsBuilder>(opt =>
            {
#if DEBUG
                // This will log EF-generated SQL commands to the console
                opt.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
                // This will log the actual params for those commands to the console
                opt.EnableSensitiveDataLogging();
                // Log more detail on errors for debugging purposes
                opt.EnableDetailedErrors();
#endif
            });

            // Add School Context
            services.AddDbContext<Data.SchoolContext>(opt =>
            {
                // Get from Tye if available, otherwise from appsettings
                var connectionString = Configuration.GetConnectionString("sqlserver-usidore") ?? "name=University";
                opt.UseSqlServer(connectionString, opt => opt.EnableRetryOnFailure(10));
                debugLogging(opt);
            }, ServiceLifetime.Transient);
        }

        private void TryRunMigrations(IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService<IConfig>();

            // Do DB migratons, if enabled
            if (config?.RunDbMigrations == true)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Data.SchoolContext>();
                    db.Database.Migrate();
                }
            }
        }

        private void TrySeedDatabase(IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService<IConfig>();

            // Seed the database, if enabled
            if (config?.SeedDatabase == true)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<Data.SchoolContext>();
                    Data.SchoolContextDbInitializer.Initialize(dbContext);
                }
            }
        }
    }
}
```

Everything should be relatively self-explanatory.  The main bit that uses `Tye` is this line in `AddDbContexts`:

```csharp
var connectionString = Configuration.GetConnectionString("sqlserver-usidore") ?? "name=University";
```

Tye makes the *sqlserver-usidore* connection string available via the definition in `tye.yaml`.  If we fail to get that connection string, we fall back to the `appSettings.json` variant:

```json
{
  "ConnectionStrings": {
    "University": "Server=localhost,21433;Database=Usidore;MultipleActiveResultSets=true;User ID=sa;Password=Password1!"
  }
}
```

This pattern allows you to run in both Tye and non-Tye environments, as needed.

## What Did We Gain?
First, we've got a fully functional, self-contained instance of SQL Server 2019.  You can connect to it with SSMS and tinker with it just as you would any other SQL Server instance.  This database gets completely torn down and recreated every time Tye is restarted.  Nobody else will be monkeying with it while you're debugging, so you can be sure everything you see in the database is coming from your local dev environment.

Second, as with everything in Tye, you get a log viewer for this SQL Server instance.  This is cool for detecting weird stuff that might be happening on the server itself.

Third, while not explicitly something that Tye gives you, you do get the ability to run migrations and seeding to get this database into a known state each time you start a debugging session.  That said, Tye does make it super easy to move into this kind of space, so I think that it deserves a little bit of credit for making this a really easy, viable option for local development.

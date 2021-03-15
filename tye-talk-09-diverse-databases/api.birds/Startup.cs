using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;

namespace api.birds
{
    public class Startup
    {
        private ILogger _startupLogger;
        private static object _seedLock = new object();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.birds v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            TrySeedDatabase(app);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _startupLogger = LoggerFactory.Create(x => x.AddConsole()).CreateLogger<Startup>();

            AddDbContexts(services);
            ScanAndRegister(services);
            DecorateRegistrations(services);

            var config = Configuration.GetSection("CustomConfig")?.Get<Config>();
            config.MongoDbConnectionString = Configuration.GetConnectionString("mongodb-arnie") ?? Configuration.GetConnectionString("birds");
            services.AddSingleton<IConfig>(config);

            services.AddAutoMapper(typeof(MappingProfiles.ModelsToResources));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api.birds", Version = "v1" });
            });
        }

        private void AddDbContexts(IServiceCollection services)
        {
            services.AddSingleton<IConfig>(Configuration.GetSection("CustomConfig")?.Get<Config>());

        }

        private void DecorateRegistrations(IServiceCollection services)
        {
        }

        private void ScanAndRegister(IServiceCollection services)
        {
            foreach (var namespacePrefix in new[] { "api.birds" })
            {
                services.Scan(s =>
                {
                    s.FromApplicationDependencies(p => p.FullName.StartsWith(namespacePrefix, StringComparison.InvariantCultureIgnoreCase))
                        .AddClasses()
                        .AsMatchingInterface();
                });
            }
        }

        private void TrySeedDatabase(IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService<IConfig>();

            // Do DB migratons, if enabled
            if (config?.SeedDatabase == true)
            {
                lock (_seedLock)
                {
                    _startupLogger.LogInformation("Beginning database migrations");
                    using var scope = app.ApplicationServices.CreateScope();

                    var client = new MongoClient(config.MongoDbConnectionString);
                    var database = client.GetDatabase("birdsApi");
                    var birds = database.GetCollection<Models.Bird>("birds");
                    birds.InsertMany(
                    new Models.Bird[]
                    {
                        new Models.Bird { Id = Guid.NewGuid(), Name = "Mourning Dove", WingSpan = 0.5 },
                        new Models.Bird { Id = Guid.NewGuid(), Name = "Northern Cardinal", WingSpan = 0.6 },
                        new Models.Bird { Id = Guid.NewGuid(), Name = "American Robin", WingSpan = 0.7 },
                        new Models.Bird { Id = Guid.NewGuid(), Name = "American Crow", WingSpan = 0.8 },
                        new Models.Bird { Id = Guid.NewGuid(), Name = "Blue Jay", WingSpan = 0.9 },
                        new Models.Bird { Id = Guid.NewGuid(), Name = "Song Sparrow", WingSpan = 1.0 },
                        new Models.Bird { Id = Guid.NewGuid(), Name = "Red-winged Blackbird", WingSpan = 1.2 },
                        new Models.Bird { Id = Guid.NewGuid(), Name = "European Starling", WingSpan = 1.3 },
                        new Models.Bird { Id = Guid.NewGuid(), Name = "American Goldfinch", WingSpan = 1.4 },
                        new Models.Bird { Id = Guid.NewGuid(), Name = "Canada Goose", WingSpan = 1.5 },
                    });
                    _startupLogger.LogInformation("Completed database seeding");
                }
            }
        }
    }
}
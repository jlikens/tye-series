using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System;
using System.Collections.Generic;
using System.Net;

namespace api.books
{
    public class Startup
    {
        private ILogger _startupLogger;

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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.books v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            TryRunMigrations(app);
            TrySeedDatabase(app);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _startupLogger = LoggerFactory.Create(x => x.AddConsole()).CreateLogger<Startup>();

            AddDbContexts(services);
            AddDistributedCache(services);
            ScanAndRegister(services);
            DecorateRegistrations(services);
            AddRedLock(services);

            services.AddSingleton<IConfig>(Configuration.GetSection("CustomConfig")?.Get<Config>());
            services.AddAutoMapper(typeof(MappingProfiles.ModelsToResources));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api.books", Version = "v1" });
            });
        }

        private void AddRedLock(IServiceCollection services)
        {
            var redisConnectionString = Configuration.GetConnectionString("redis") ?? Configuration.GetConnectionString("HostedRedis");
            var host = redisConnectionString.Split(':')[0];
            var port = redisConnectionString.Split(':')[1];

            var endPoints = new List<RedLockEndPoint>
            {
                new DnsEndPoint(host, int.Parse(port)),
            };
            var redlockFactory = RedLockFactory.Create(endPoints);
            services.AddSingleton<RedLockFactory>(redlockFactory);
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
            services.AddDbContextPool<Data.BookContext>(opt =>
            {
                // Get from Tye if available, otherwise from appsettings
                var connectionString = Configuration.GetConnectionString("mysql-jamilious") ?? Configuration.GetConnectionString("Books");
                Console.WriteLine(connectionString);
                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), opt => opt.EnableRetryOnFailure(10));
                debugLogging(opt);
            });
        }

        private void AddDistributedCache(IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(o =>
            {
                o.Configuration = Configuration.GetConnectionString("redis") ?? Configuration.GetConnectionString("HostedRedis");
                _startupLogger.LogInformation("Redis configuration", o.Configuration);
            });
        }

        private void DecorateRegistrations(IServiceCollection services)
        {
            services.Decorate<Services.IBookService, Services.CachingBookService>();
        }

        private void ScanAndRegister(IServiceCollection services)
        {
            foreach (var namespacePrefix in new[] { "api.books" })
            {
                services.Scan(s =>
                {
                    s.FromApplicationDependencies(p => p.FullName.StartsWith(namespacePrefix, StringComparison.InvariantCultureIgnoreCase))
                        .AddClasses()
                        .AsMatchingInterface();
                });
            }
        }

        private void TryRunMigrations(IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService<IConfig>();

            // Do DB migratons, if enabled
            if (config?.RunDbMigrations == true)
            {
                _startupLogger.LogInformation("Beginning database migrations");
                using var redlock = app.ApplicationServices.GetService<RedLockFactory>().CreateLock($"api.books.{nameof(TryRunMigrations)}", TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(1));
                if (redlock.IsAcquired)
                {
                    using var scope = app.ApplicationServices.CreateScope();

                    var db = scope.ServiceProvider.GetRequiredService<Data.BookContext>();
                    db.Database.Migrate();
                }
                else
                {
                    throw new Exception("Unable to acquire Redis lock.  Migrations cannot be safely run!");
                }
                _startupLogger.LogInformation("Completed database migrations");
            }
        }

        private void TrySeedDatabase(IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetService<IConfig>();

            // Seed the database, if enabled
            if (config?.SeedDatabase == true)
            {
                _startupLogger.LogInformation("Beginning database seeding");
                using var redlock = app.ApplicationServices.GetService<RedLockFactory>().CreateLock($"api.books.{nameof(TrySeedDatabase)}", TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(1));
                if (redlock.IsAcquired)
                {
                    using var scope = app.ApplicationServices.CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<Data.BookContext>();
                    Data.SchoolContextDbInitializer.Initialize(dbContext);
                }
                else
                {
                    throw new Exception("Unable to acquire Redis lock.  Database seeding cannot be safely run!");
                }
                _startupLogger.LogInformation("Completed database seeding");
            }
        }
    }
}
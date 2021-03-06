using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace api.university
{
    public class Startup
    {
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.university v1"));
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
            services.AddSingleton<IConfig>(Configuration.GetSection("CustomConfig")?.Get<Config>());

            AddDbContexts(services);

            services.AddAutoMapper(typeof(MappingProfiles.ModelsToResources));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api.university", Version = "v1" });
            });
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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace frontend.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddHttpClient<api.personApi.IPersonApiClient, api.personApi.PersonApiClient>(client =>
            {
                client.BaseAddress = Configuration.GetServiceUri("api-person");
            });
            services.AddHttpClient<api.todoApi.ITodoApiClient, api.todoApi.TodoApiClient>(client =>
            {
                client.BaseAddress = Configuration.GetServiceUri("api-todo");
            });
            services.AddHttpClient<api.universityApi.IUniversityApiClient, api.universityApi.UniversityApiClient>(client =>
            {
                client.BaseAddress = Configuration.GetServiceUri("api-university");
            });
            services.AddHttpClient<api.weatherApi.IWeatherApiClient, api.weatherApi.WeatherApiClient>(client =>
            {
                client.BaseAddress = Configuration.GetServiceUri("api-weather");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}

# tye-talk-2020-05-tye-ahoy
If you made it this far: CONGRATULATIONS!!  We're finally going to do something with Tye!  If you will recall, we left off in our [previous example](../tye-talk-2020-04-intertwined) with several co-dependent applications: a front-end web app and three microservices.  We made the choice to tie a bunch of these together to illustrate how easy it is for microservice development to become difficult when it comes to your local dev environment.

## Moving from "4 to 5"
I see a lot of "samples" that don't really explain how to get from point A to point B, which is... annoying.  I hope to not make that same omission here.  In order to get Tye up and running with [the previous example](../tye-talk-2020-04-intertwined), here's what you need to do.

### Initializing Tye
1. Download the source for [the previous example](../tye-talk-2020-04-intertwined) (hereafter referred to as The Source)
1. Install [Tye](https://github.com/dotnet/tye/blob/master/docs/getting_started.md)
1. Open a developer command prompt and navigate to The Source **OR**
1. Run `tye init`, which will create a `tye.yaml` file in the root
1. To see what the above command did, open `tye.yaml` in your favorite editor
1. Note that it detected four applications and named them based on the name of the csproj files:
```yaml
# tye application configuration file
# read all about it at https://github.com/dotnet/tye
#
# when you've given us a try, we'd love to know what you think:
#    https://aka.ms/AA7q20u
#
name: tye-talk-2020-05-tye-ahoy
services:
- name: api-person
  project: api.person/api.person.csproj
- name: api-todo
  project: api.todo/api.todo.csproj
- name: api-weather
  project: api.weather/api.weather.csproj
- name: frontend-client
  project: frontend/Client/frontend.Client.csproj
- name: frontend-server
  project: frontend/Server/frontend.Server.csproj
```

### Updating The Code
Now that we've got Tye installed and initialized for the solution, we need to make a few code changes to take advantage of the Tye magic.  First, we'll move towards using dependency injection for our API HTTP clients so that we can configure them from our `Startup` classes.

#### Frontend
After adding the `Microsoft.Tye.Extensions.Configuration` NuGet package (note: as of this writing, this package is still in pre-release, so you'll need to enable installation of pre-release packages).  One of the main extension methods Tye brings is `IConfiguration.GetServiceUri(string)`, which dips into the configuration elements that Tye injects into the environment runtime.  We can refer to any of the projects in `tye.yaml` by name, as follows:

```csharp
namespace frontend.Server
{
    public class Startup
    {
        ...
        
        public void ConfigureServices(IServiceCollection services)
        {
            ...
            
            services.AddHttpClient<api.personApi.IPersonApiClient, api.personApi.PersonApiClient>(client =>
            {
                client.BaseAddress = Configuration.GetServiceUri("api-person");
            });
            services.AddHttpClient<api.todoApi.ITodoApiClient, api.todoApi.TodoApiClient>(client =>
            {
                client.BaseAddress = Configuration.GetServiceUri("api-todo");
            });
            services.AddHttpClient<api.weatherApi.IWeatherApiClient, api.weatherApi.WeatherApiClient>(client =>
            {
                client.BaseAddress = Configuration.GetServiceUri("api-weather");
            });
            
            ...
        }
        
        ...
    }
}
```

Here, we've configured our services for our three API HTTP clients so that we can inject them, and we've set their `BaseAddress` to the URI that Tye automagically sets up for each of the API projects.

Next, we'll want to take advantage of our injected HTTP clients by changing the controllers as follows (update the remaining controllers in a similar fashion):

```csharp
namespace frontend.Server.Controllers
{
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<PersonResource>> Get([FromServices] api.personApi.IPersonApiClient apiClient)
        {
            ...
            
            var people = await apiClient.PersonAllAsync();

            ...
        }
    }
}

```

#### Microservices
Similarly, we do the same basic configuration for each of the microservices after adding in the `Microsoft.Tye.Extensions.Configuration` NuGet package:

```csharp
namespace api.person
{
    public class Startup
    {
        ...
        
        public void ConfigureServices(IServiceCollection services)
        {
            ...
            
            services.AddHttpClient<api.clients.ITodoApiClient, api.clients.TodoApiClient>(client =>
            {
                client.BaseAddress = Configuration.GetServiceUri("api-todo");
            });
            
            ...
        }
        
        ...
    }
}
```

```csharp
namespace api.todo
{
    public class Startup
    {
        ...
        
        public void ConfigureServices(IServiceCollection services)
        {
            ...

            services.AddHttpClient<api.client.IWeatherApiClient, api.client.WeatherApiClient>(client =>
            {
                client.BaseAddress = Configuration.GetServiceUri("api-weather");
            });
            
            ...
        }
        
        ...
    }
}

```

And then we'll want to update the controllers to use the configured HTTP clients:
```csharp
namespace api.person.Controllers
{
    public class PersonController : ControllerBase
    {
        private readonly PersonContext _context;
        private readonly ITodoApiClient _todoApiClient;

        public PersonController(PersonContext context, api.clients.ITodoApiClient todoApiClient)
        {
            _context = context;
            _todoApiClient = todoApiClient;
        }

        ...

        private async Task<IEnumerable<TodoItem>> GetRandomTodoItems()
        {
            var rnd = new Random();
            var todoItems = await _todoApiClient.TodoItemsAllAsync();
            return todoItems.Take(rnd.Next(0, todoItems.Count() + 1));
        }
        
        ...
    }
}
```

```csharp
namespace api.todo.Controllers
{
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IWeatherApiClient _weatherApiClient;

        public TodoItemsController(TodoContext context, api.client.IWeatherApiClient weatherApiClient)
        {
            _context = context;
            _weatherApiClient = weatherApiClient;
        }

        ...
        
        private async Task<WeatherForecast> GetRandomWeatherForecast()
        {
            var rnd = new Random();
            var weatherForecasts = await _weatherApiClient.WeatherForecastAsync();
            return weatherForecasts.ToArray()[rnd.Next(0, weatherForecasts.Count())];
        }
    }
}
```

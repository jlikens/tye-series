# And Down We Go
As our business processes become more complex, so, too, does the code that supports these processes.  We now find ourselves in a place where we need our Person concept to hold Todo instances, and each Todo instance needs to hold a WeatherForecast instance.

In the ideal world, each domain would manage these child dependencies on their own so that each microservice would be entirely stand-alone.  More often, though, dev units wind up connecting microservice A to microservice B, and so on.  This is especially prevalent in cases where an organization hasn't decided how to provide app-specific databases, how to deal with eventual consistency, etc.  

We're going to connect our Person microservice to our Todo microservice, and our Todo microservice to our Weather microservice.  Again, ideally, this would be avoided, but because it is a common scenario, we're going to let this anti-pattern seep into our code.

So at this point, we've got a fairly complicated situation.  When you run this code, you'll notice that it spools up a whole bunch of console windows (one for the front-end, and one each for each microservice).  We also need to maintain the hosts/ports across three different projects: the front-end, which talks to everything, needs an endpoint for each microservice; the Person microservice needs an endpoint for the Todo microservice; and the Todo microservice needs an endpoint for the Weather microservice.

It should be fairly apparent that, even with just three microservices and one front-end, we've already got a fair amount of yucky complexity to manage.  Not only do we have to make sure that the endpoints for each API  are configured correctly for each client, we also have to make sure we have a good understanding about how all of these things are interconnected.  Bear in mind, we are still working in a super-simple scenario in which one single solution holds all of the interdependent projects.

Take a look at the spots where we have to point a client to an API.  In our next example, we'll finally bring in Tye and see how it makes life easier, particularly with respect to easing the management of these connections.

### Endpoints
As an example of our growing complexity, notice how we have to maintain `https://localhost:5005` as the endpoint for our Todo API in two places:

```csharp
namespace api.person.Controllers
{
    public class PersonController : ControllerBase
    {
        ...
        
        private async Task<IEnumerable<TodoItem>> GetRandomTodoItems()
        {
            ...
            var httpClient = new HttpClient();
            var client = new api.clients.TodoApiClient("https://localhost:5005", httpClient);
            ...
        }
        
        ....
    }
}
```
```csharp
namespace frontend.Server.Controllers
{
    public class TodoController : ControllerBase
    {
        ...
        
        [HttpGet]
        public async Task<IEnumerable<TodoItemResource>> Get()
        {
            ...
            var httpClient = new HttpClient();
            var client = new api.todoApi.TodoApiClient("https://localhost:5005", httpClient);
            ...
        }
        
        ...
    }
}
```

Obviously, in the real world, we wouldn't hardcode our endpoints into the code, and we are only doing so here for simplicity's sake.  It still serves to illustrate the point that we have multiple places, in multiple projects, that need to refer to the same configuration element.

Within a single project, this is easy to manage: just add something to your `appSettings.json` to point to the correct endpoint, and you're good to go.  But how do you make sure this endpoint is synchronized across multiple different projects?

# tye-talk-2020-08-replication
One of the more challenging aspects of distributed computing is dealing with multiple instances of the same resources.  The most common example of this is load balancing.  In a production-size environment, you'll generally be running each layer of your application multiple times, using a load balancer to spread out requests among all of the instances of each resource.

Setting this kind of environment up in the cloud is pretty easy.  Setting one up locally, however, generally takes quite a bit of effort.  Why would you want to do this?  What if you wanted to test how session behaves in your application when dealing with multiple web servers?  How about testing distributed access to a common resource?  What about testing distributed caching?

In the not-too-distant past, dealing with bugs or performance issues in these types of distributed scenarios was difficult and annoying, often requiring a whole bunch of hoops to be jumped through just to run a single test locally.

With Tye replicas, we can quickly and easily spool up as many instances of any of our services as we need.  In this example, we'll take a look at both distributed locking and distributed caching.

## Some New Stuff
We've brought a few new things on board for this example.  As our application examples get more complicated, we'll do our best to make sure the code we add isn't just a bunch of hacks crammed in.  While we aim to keep things very simple, we do want to espouse as many clean patterns and practices as possible.  With that in mind, let's take a look at the new bits.

### Scutor
First, there's [Scrutor](https://github.com/khellang/Scrutor), which provides:

> Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection

The vast majority of all dependency injection scenarios revolve around creating an interface, say `IFoo`, and a single implementaton of that interface, `Foo`.  Scrutor makes it quick and easy to automatically scan the assemblies in your application and automatically register all `IFoo` -> `Foo` registrations.  Additionally, you can use it to decorate an existing `IFoo` -> `Foo` with `FooDecorated`, allowing you to shim in an overarching implementation of `IFoo` (more on this later).

### Redis
Next, we've added [Redis](https://redis.io/) to the mix.  Redis is a distributed cache.  For the uninitiated, think of this as the same as a normal in-process memory cache, except it can be shared across an arbitrary number of client applications.  This means that multiple applications, and multiple instances of a single application, can all participate in working with a shared memory cache.  This enables scenarios whereby all instances of a particular app can ensure they have the latest and greatest data, regardless of which instance was responsible for first loading that data into the cache.

On a related note, we will also be using an implementation of the [Redlock algorithm](https://redis.io/topics/distlock).  This essentially enables the equivalent of thread-locking at the distributed system.  For example, assume you have two instances of an application, each of which performs the same startup code.  This startup code cannot safely be run multiple times simultaneously.  How do we prevent instance A from smashing instance B's startup, and vice versa?

### Redis Commander
One really cool aspect of Tye is its ability to automatically pull down and run utility containers.  We saw a bit of this with our [Tye++](../tye-talk-2020-06-tye-plus-plus) example, wherein we pulled in a distributed tracing tool (Zipkin) and a log aggregator (ELK) with almost no effort.  In this example, we'll be pulling in [Redis Commander](https://www.bing.com/search?form=MOZLBR&pc=MOZI&q=redis+commander) and hook it up to our local Redis instance just in case we want to poke around in the Redis cache.

### Replicas
With a single line of yaml, Tye lets you turn on as many copies of a service as you want.  We'll be using this feature in this example to show how to test distributed locking and distributed caching within our `api.university` project.

## Tye Updates
Let's take a peek at what we've added to our `tye.yaml` from the [previous example](../tye-talk-2020-07-sql-server).

```yaml
name: tye-talk-2020-08-replication
...
services:
...
- name: api-university
  project: api.university/api.university.csproj
  replicas: 3
- name: redis
  image: redis
  bindings:
  - port: 36379
    containerPort: 6379
    connectionString: "${host}:${port}" 
- name: redis-commander
  image: rediscommander/redis-commander:latest
  bindings:
  - port: 38081
    protocol: http
    containerPort: 8081
  env:
  - name: REDIS_HOSTS
    value: local:redis:6379
```

First note that we added `replicas: 3` to `api-university`.  This will launch three instances of this service when we run Tye.

Next, note that we pulled in both `redis` and `rediscommander` by referencing these docker images.  Depending on your environment's reserver ports, you may need to change the `port` for `redis`.

> Aside: On Windows, to check your reserved ports, run this command: `netsh int ipv4 show excludedportrange protocol=tcp`

## Code Updates

### Caching Layer

### Startup Code

* Scrutor
* Add replicas to university service
* Add Redis to tye
* Add redis commander to tye
* Add redis to startup
* Add caching decorator
* Add redlock for db migrations and seeding
* Added very basic caching implementation

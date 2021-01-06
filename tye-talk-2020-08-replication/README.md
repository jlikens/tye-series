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

In short, it's easy, unobstrusive, and simple to get convention-based `IFoo` -> `Foo` registrations as well as decorating existing registrations (more on this later).

### Redis
Next, we've added [Redis](https://redis.io/) to the mix.  For the uninitiated, think of Redis as a cache that can be accessed by anyone interested in sharing a blob of memory.  This means that multiple applications, and multiple instances of a single application, can all participate in a shared memory cache.  This enables scenarios whereby all instances of a particular app can ensure they have the latest and greatest data, regardless of which instance was responsible for first loading that data into the cache.

On a related note, we will also be using an implementation of the [Redlock algorithm](https://redis.io/topics/distlock).  This essentially enables the equivalent of thread-locking at the distributed system.  For example, assume you have two instances of an application, each of which performs the same startup code.  This startup code cannot safely be run multiple times simultaneously.  How do we prevent instance A from smashing instance B's startup, and vice versa?

### Redis Commander

* Scrutor
* Add replicas to university service
* Add Redis to tye
* Add redis commander to tye
* Add redis to startup
* Add caching decorator
* Add redlock for db migrations and seeding
* Added very basic caching implementation

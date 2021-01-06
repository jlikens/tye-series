# tye-series
A code series that shows practical, real-world scenarios involving distributed applications and Project Tye.  In general, each example builds upon the ideas of th previous one, but each is also designed to run on its own.

## The Story
This series started life as the demo code for a dev talk.  The first six parts are centered around a journey from a simple monolith to a microservice architecture, using a contrived set of business rules to make things a little complicated.

## Pre-requisites 
The basics:
* Install [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
* Install [Project Tye](https://github.com/dotnet/tye)

For examples that use containers:
* Install [Docker Desktop](https://www.docker.com/products/docker-desktop) for your OS

## The Series
* [tye-talk-2020-01-simple](tye-talk-2020-01-simple)
  * A basic monolithic application, which is essentially just the standard [Blazor WASM Weather App](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro)
* [tye-talk-2020-02-microservice](tye-talk-2020-02-microservice)
  * We add in our first microservice
* [tye-talk-2020-03-more-microservices](tye-talk-2020-03-more-microservices)
  * We add in a couple more microservices
* [tye-talk-2020-04-intertwined](tye-talk-2020-04-intertwined)
  * We spaghettify our microservices
* [tye-talk-2020-05-tye-ahoy](tye-talk-2020-05-tye-ahoy)
  * We see how Tye can help simplify our spaghetti
* [tye-talk-2020-06-tye-plus-plus](tye-talk-2020-06-tye-plus-plus)
  * We use Tye to quickly add in [Zipkin](https://zipkin.io/) and [Elastic Stack](https://www.elastic.co/elastic-stack) to take our local debugging and dependency monitoring experience to the next level
* [tye-talk-2020-07-sql-server](tye-talk-2020-07-sql-server)
  * We add in a SQL Server container, run some database migrations against it, and have our own freshly initialized local development database
* [tye-talk-2020-08-replication](tye-talk-2020-08-replication)
  * We dive into local load balancing to test distributed locking and distributed caching with Redis

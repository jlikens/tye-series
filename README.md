# Tye Series
This is a code series that shows practical, real-world scenarios involving distributed applications and [Project Tye](https://github.com/dotnet/tye).  In general, each example builds upon the ideas and code of the previous one, but each is also designed to run on its own.  My goal is to provide an example that evolves over time rather than just the "end result" so you can see what changes from scenario to scenario.

The first four parts of the series are there to set up common scenarios you will encounter when developing distributed apps.  If you want to skip right to the good stuff, hop into the [5th](tye-talk-05-tye-ahoy) in the series where we actually start using Tye.

I'm a relative newcomer to the world of Tye, so if anyone has any feedback about anything, please [let me know](../../issues)!  Also, if you wanna keep track of what's going on in the Tye codebase, the Tye team claims that they'll post [here](https://github.com/dotnet/tye/issues/251) every two weeks-ish (YMMV).

## The Story
This series started life as the demo code for a dev talk.  Each piece in the series builds upon the ideas of the previous.  We start with a simple little Blazor app.  Over time, we add in co-dependent microservices, distributed monitoring and logging, local databases, local load balancing, and more!  After the 4th in the series, we start to use Tye to make our lives easier when it comes to inner-loop development of this distributed app.

## Pre-requisites 
The basics:
* Install [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
  * Tye does not need .NET 5.0, but all of the examples are running on .NET 5.0 so you'll need it
* Install [Project Tye](https://github.com/dotnet/tye)
  * The main tooling for Tye
 
For examples that use containers:
* Install [Docker Desktop](https://www.docker.com/products/docker-desktop) for your OS

For examples that use Kubernetes:
* A Kubernetes cluster, such as [Azure Kubernetes Service](https://azure.microsoft.com/en-us/services/kubernetes-service/)
* The [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli) utility
* [kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl-windows/)

## Further Reading & Resources
Here are some places to go that delve a bit deeper into the various technologies used it this series.
* [Introducing Project Tye](https://devblogs.microsoft.com/aspnet/introducing-project-tye/)
* [Building Microservices with Tye - March 2021 Update](https://www.youtube.com/watch?v=m4VsOdIT1O4)
* [Tye and Kubernetes](https://www.youtube.com/watch?v=prbYvVVAcRs)
* [How Dell Botched Their Initial Microservice Implementation](https://www.youtube.com/watch?v=gfh-VCTwMw8)
* [Basic Info on Kubernetes](https://kubernetes.io/docs/tutorials/kubernetes-basics/)
* [Docker Hub](https://hub.docker.com/)
* [Overview of Microservices](https://en.wikipedia.org/wiki/Microservices)
* [Pluralsight .NET Microservices Path](https://www.pluralsight.com/paths/net-microservices)
* [Intro to Zipkin](https://www.youtube.com/watch?v=jkSm-652UPo)
* [Intro to Elastic Search Series](https://www.youtube.com/watch?v=GE_Nf9OHf7g&list=PL_mJOmq4zsHbsqFTG0toPRz58uSuRiBK8)

## The Series
* [tye-talk-01-simple](tye-talk-01-simple)
  * A basic monolithic application, which is essentially just the standard [Blazor WASM Weather App](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro)
* [tye-talk-02-microservice](tye-talk-02-microservice)
  * We add in our first microservice
* [tye-talk-03-more-microservices](tye-talk-03-more-microservices)
  * We add in a couple more microservices
* [tye-talk-04-intertwined](tye-talk-04-intertwined)
  * We spaghettify our microservices
* [tye-talk-05-tye-ahoy](tye-talk-05-tye-ahoy)
  * We see how Tye can help simplify our spaghetti
* [tye-talk-06-tye-plus-plus](tye-talk-06-tye-plus-plus)
  * We use Tye to quickly add in [Zipkin](https://zipkin.io/) and [Elastic Stack](https://www.elastic.co/elastic-stack) to take our local debugging and dependency monitoring experience to the next level
* [tye-talk-07-sql-server](tye-talk-07-sql-server)
  * We add in a SQL Server container, run some database migrations against it, and have our own freshly initialized local development database
* [tye-talk-08-replication](tye-talk-08-replication)
  * We dive into local load balancing to test distributed locking and distributed caching with Redis
* [tye-talk-09-diverse-databases](tye-talk-09-diverse-databases)
  * Microservices are supposed to have their own isolated data stores.  In this episode, we look at how to spool up a whole bunch of popular data storage platforms  (SQL Server, MySQL, PostgreSQL, MariaDB, and MongoDB)
* [tye-talk-10-deploy-to-aks](tye-talk-10-deploy-to-aks)
  * We take a peek at Tye's Kubernetes deployment capabilities by sending a slimmed down version of our app to Azure Kubernetes Service

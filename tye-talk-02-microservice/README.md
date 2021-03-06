# The Fracturing Begins
As we start to get more users and our business grows, all of the "fun" that comes with that kind of growth starts to arrive.  The new security team has mandated that data access cannot be done from the same physical tier as the web front-end.  With that in mind, we move our Weather domain into a microservice and host it separately.  

We've still got everything in the same solution, but we deploy the front-end and service layers to different tiers.  Because we're still in this simple space, we don't really need Tye, but note that we've started down the path of having cross-project HTTP dependencies in the form of the front-end needing to call the Weather microservice.  Keep this in mind as we move into the [next example](../tye-talk-2020-03-more-microservices).

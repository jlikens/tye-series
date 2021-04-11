# Growth Brings Sprawl
Now our business starts to get more interesting.  Our users want some personalization, and they also need to be able to manage to-do lists (because, you know, that makes sense, right??).  We decide to add in the domains of People and Todos, and since we've already made a microservice pattern, we add two more projects to represent the microservices for these new domains.

Our world is still relatively simple, but we do now have to manage three different endpoints from the front-end project (one for each of our microservices).  In our [next example](../tye-talk-2020-intertwined), we'll make a mess of things by hooking different microservices together.

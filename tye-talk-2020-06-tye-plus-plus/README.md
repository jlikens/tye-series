# tye-talk-2020-06-tye-plus-plus
Now that we've gotten Tye running in our [previous example](../tye-talk-2020-05-tye-ahoy), we can start doing some really cool things.  One of the main complications when developing and debugging distributed applications is having a clean, isolated environment where you can see exactly what's going on for your calls, and *only* your calls.  Two applications that assist with this are [ZipKin](https://zipkin.io/) for distributed tracing, and [Elastic Stack](https://www.elastic.co/elastic-stack) for log aggregation.

Normally, setting these up involves either installing the apps locally, or, in more advanced cases, downloading Docker images for each and setting up the appropriate containers.  Both of these setup paths take some time, and you have to do them every time a new dev machine comes online.

With Tye, we can easily add these dependencies with just a couple of tweaks to our `tye.yaml` file, and, because of Tye magic, have them automatically bubbled into our Tye runtime so we don't have to mess around with configuration.

If you've ever worked on any serious distributed application, you will immediately recognize the benefit of this setup.

## The Setup
First, make sure you have [Docker Desktop](https://www.docker.com/products/docker-desktop) installed (or whatever flavor of Docker you want, if you have something else running already).  **NOTE:** If you develop in a super-locked-down development environment where you aren't allowed to do adminstrative things and you aren't allowed to mess with antivirus exclusions, you're going to have a bad time. :(  Sadly, there isn't much you can do other than plead to your Ops folks for elevated permissions.

All we need to do is add the following lines to our `tye.yaml` file:
```yaml
extensions:
- name: zipkin
- name: elastic
  logPath: ./.logs
```

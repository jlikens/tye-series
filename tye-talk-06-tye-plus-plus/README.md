# Supercharging the Inner Loop
Now that we've gotten Tye running in our [previous example](../tye-talk-05-tye-ahoy), we can start doing some really cool things.  One of the main complications when developing and debugging distributed applications is having a clean, isolated environment where you can see exactly what's going on for your calls, and *only* your calls.  Two applications that assist with this are [Zipkin](https://zipkin.io/) for distributed tracing, and [Elastic Stack](https://www.elastic.co/elastic-stack) (aka ELK) for log aggregation.

Normally, setting these up involves either installing the apps locally, or, in more advanced cases, downloading Docker images for each and setting up the appropriate containers.  Both of these setup paths take some time, and you have to do them every time a new dev machine comes online.

With Tye, we can easily add these dependencies with just a couple of tweaks to our `tye.yaml` file, and, because of Tye magic, have them automatically bubbled into our Tye runtime so we don't have to mess around with configuration.

If you've ever worked on any serious distributed application, you will immediately recognize the benefit of this setup.

## The Setup
First, make sure you have [Docker Desktop](https://www.docker.com/products/docker-desktop) installed (or whatever flavor of Docker you want, if you have something else running already).  **NOTE:** If you develop in a super-locked-down development environment where you aren't allowed to do adminstrative things and you aren't allowed to mess with antivirus exclusions, you're going to have a bad time. :anguished:  Sadly, there isn't much you can do other than plead to your Ops folks for elevated permissions.

All we need to do is add the following lines to our `tye.yaml` file:
```yaml
extensions:
- name: zipkin
- name: elastic
  logPath: ./.logs
```

Give it a `tye run` and, if all goes well, you should be able to load up your Tye Dashboard and see new entries for *zipkin* and *elastic*!

### Some Elastic Weirdness
When you first try to load up your ELK instance, you may see this in your logs:
```
[elastic_3c3cdec4-8]: ERROR: [1] bootstrap checks failed
[elastic_3c3cdec4-8]: [1]: max virtual memory areas vm.max_map_count [65530] is too low, increase to at least [262144]
[elastic_3c3cdec4-8]: ERROR: Elasticsearch did not exit normally - check the logs at /var/log/elasticsearch/elasticsearch.log
```

If so, you'll need to bump up the `vm.max_map_count` setting in WSL.  In Windows, you can do this from a PowerShell admin console in one of two ways.

#### Option 1: Temporary
The following will bump up the `vm.max_map_count` until the host is rebooted:

```powershell
wsl -d docker-desktop
sysctl -w vm.max_map_count=262144
exit
```

Restart Docker Desktop, re-run `tye run`, and you should be all set.

#### Option 2: Permanent
The following will bump up the `vm.max_map_count` forever.  Note that this code lets you use Notepad in Windows, but if you're confortable with vi, you can just use that.

```powershell
wsl -d docker-desktop
alias notepad="/mnt/host/c/windows/system32/notepad.exe"
cd /etc
notepad sysctl.conf
```

Add a single line to the file and save it.

```text
vm.max_map_count=262144
```

Restart Docker Desktop, re-run `tye run`, and you should be all set.

## Using Zipkin
Zipkin is pretty neat, and the ease with which Tye can add it to your distributed environment is pretty amazing.  With this example running, load up your Tye Dashboard and click on the link in the *Bindings* column in the `zipkin` row.  Then, click the *Run Query* button aaaaand.... nothing!  Well, that's because you haven't made any requests to anything yet.

Back in the Tye Dashboard, load up the HTTPS binding for *frontend-server*.  Add a few todos, add a few people, and load up the weather page.  Then, hop back over to the Zipkin dashboard and click *Run Query*.  Now you should see some entries, and you can click on any to drill down.  I encourage you to poke around on the [Zipkin docs](https://zipkin.io/) to understand how it works, but here are the main things to know.

First, for any call, you can see the call time and dependency chain.  After adding three people and three todos, I looked at the call for the People page and got this little gem:

![Zipkin call trace](https://i.imgur.com/nX4EUrM.png)

Pretty cool, right?

Another awesome feature is the animated dependency graph.  Click the *Dependencies* link in the toolbar, and you'll see something like this:
![Zipkin dependency graph](https://i.imgur.com/WtwRj02.gif)

Good stuff!

## Using ELK
Getting up and running with ELK takes a little bit more elbow grease (but not *too* much).  First, let's quickly look back at what we added to our `tye.yaml` file:
```yaml
extensions:
- name: zipkin
- name: elastic
  logPath: ./.logs
```
Notice the *logPath* variable in there.  From the Tye docs:

> :bulb: Tye can successfully launch Elastic stack without `logPath`, but ... It's *highly* recommended that you specify a path to store the logs and configuration (add to `.gitignore` if it's part of your repository). Kibana has some mandatory setup the first time you use it, and without persisting the data, you will have to go through it each time.

Pay particular attention to the .gitignore bits in there.  You (most likely) don't want your log files added to your git repo, and you'll notice that this example's .gitignore has `.logs/` at the bottom.

After this, the Tye docs are a bit out of date with the (as of December 2020) Kibana interface.  Here's how to get it going for the first time (remember to hit the Blazor app at least once to get a few logs out there):

![ELK Setup](https://i.imgur.com/d0p1Q0p.gif)

As long as you don't delete your `.logs` folder, you should only have to do this initial setup one time.  At this point, you now have all sorts of super useful info that you can search through the Kibana interface.  There is so much that you can do with this, so please take a look at the [Kibana docs](https://www.elastic.co/guide/en/kibana/current/introduction.html).

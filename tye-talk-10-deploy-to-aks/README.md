
# tye-talk-2020-10-deploy-to-aks
Now it's time to take a look at deploying applications with Tye.

> :bulb: Important note: Tye does NOT deploy anything other than project nodes.  That is, it will not deploy dependent images.  These must be deployed into your cluster externally.

What you'll need:
* A Kubernetes cluster (we'll use [Azure Kubernetes Service](https://azure.microsoft.com/en-us/topic/what-is-kubernetes/), but any will work)
* The (Azure CLI)[https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli]
* (`kubectl`([https://kubernetes.io/docs/tasks/tools/install-kubectl-windows/]

## What's Changed
In the tye.yaml, we've added a couple of new nodes: `registry` and `ingress`:
```yaml
...
registry: likenstech.azurecr.io
ingress:
  - name: ingress-app
    rules:
      - path: /
        service: frontend-server
extensions:
- name: zipkin
- name: elastic
...
```

### Registry Node
The first element, `registry`, allows us to tell tye which container registry to use.

### Ingress Node
The second element, `ingress`, allows us to forward external calls to the container host to specific internal services.  In this case, we're forwarding 8080 to the main front-end applicaiton.

## Create a Kubernetes Cluster in Azure
* Create a new k8s cluster in your [Azure Portal](https://portal.azure.com)
** Set node count to 5
** Make sure to use one of the cheap VMs (as of this writing, the B2s is the cheapest at about $1USD/day)
** Turn on monitoring for now so you can see when things blow up
** Leave everything else at defaults for now
* Note that resource creation will take a few minutes

## Install kubectl
>If you already have `kubectl` installed, you can skip this step.

Tye uses some Kubernetes commands to do deployments, so you'll need to have them available to get deployments working.
Follow one of the options listed (here)[https://kubernetes.io/docs/tasks/tools/install-kubectl-windows/] to get `kubectl` installed on your system.  For reference, I used the [Chocolatey](https://chocolatey.org) option because I prefer to use `choco` when possible.

## Install Azure CLI
>If you already have `kubectl` installed, you can skip this step.

>```powershell
>Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'; rm .\AzureCLI.msi
>```

## Set Up External Dependencies
As mentioned in the intro, Tye will only deploy your projects.  It will not deploy external containers/dependencies.  While you may or may not agree with this, that is the current state of the tool.

With that in mind, let's do a little bit of a dive into Kubernetes to set up the various external dependencies we have.

> :bulb: Pro-tip: As you mess with all of this, you may find yourself looking at lots of Failed or Evicted pods.  If that happens, run `kubectl delete pods --all-namespaces --field-selector 'status.phase==Failed'` to blow them all away!

## Deploy
* Open an administrative PowerShell console
* Run the following:
```powershell
# Set variables for your Azure container registry, corresponding Azure resource group, and AKS instance
$containerRegistry = "likenstech"
$resourceGroup = "jess-sandbox"
$aksInstance = "k8s-tye-talk"
# Login to your Azure account in a browser
az login 
# Login to your Azure container registry
az acr login -n $containerRegistry
# Set the kubectl context
az aks get-credentials --resource-group $resourceGroup --name $aksInstance
# Use tye to deploy out to AKS
tye deploy --interactive
>If you get an error saying `Cannot apply manifests because kubectl is not installed.`, please follow the steps above to install `kubectl`.
```
* The first time you deploy, a few things will happen:
** First, you'll be prompted to enter a URI for Zipkin and Elastic
*** Just enter in a dummy URI (it needs to be in a proper URI format) as Tye can't deploy these and they won't be used
** Second, you'll be prompted to deploy a new ingress-nginx instance.  Choose `yes`.
*** `Deploy ingress-nginx (y/n): y`

Once it's complete, you can verify that things were deployed:
```powershell
kubectl get service
```

After everything is out there and running, you can either check your Azure portal for the external ip of the new `ingress-ngnix-controller` service or run the following command:
```powershell
kubectl get service -o wide -A
```

Look for a row that looks like this:

```text
NAMESPACE       NAME                                 TYPE           CLUSTER-IP     EXTERNAL-IP     PORT(S)                      AGE     SELECTOR
ingress-nginx   ingress-nginx-controller             LoadBalancer   10.0.15.55     52.143.254.78   80:31009/TCP,443:32249/TCP   3m      app.kubernetes.io/component=controller,app.kubernetes.io/instance=ingress-nginx,app.kubernetes.io/name=ingress-nginx
```

Load up the EXTERNAL-IP in a browser and enjoy the app!

## Undeploying
Undeploying with Tye is pretty easy.  Simply run `tye undeploy` and the apps resources will be blown away!  R
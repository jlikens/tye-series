
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
** Set node count to 3
** Make sure to use one of the cheap VMs (as of this writing, the B2s is the cheapest at about $1USD/day)
*** Note that MSSQL is a memory hog, so you may need to bump up the node count and/or the VM size to have more memory :weary:
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

### SQL Server
See https://docs.microsoft.com/en-us/sql/linux/tutorial-sql-server-containers-kubernetes?view=sql-server-ver15 for a longer discussion.

I want to get you going, so here is the condensed version.  In a PowerShell admin window, from the root of the solution, run:

```powershell
# Create a secret to store the SA password
kubectl create secret generic mssql --from-literal=SA_PASSWORD="mbGJHz11ltChdD9xLnTr"
# Apply the MSSQL storage configuration
kubectl apply -f .\sqlserver-storage.k8s.yaml
# Verify the persistent volume claim
kubectl describe pvc mssql-data
# Verify the persistent volume
kubectl describe pv
# Apply the MSSQL image configuration
kubectl apply -f .\sqlserver-instance.k8s.yaml
```

Run `kubectl get pod` every once in a while to see the status of the new container.  Once the msqsql-deployment container is `Running`, continue (this may take a few minutes if it's the first time doing so in your cluster):
```text
mssql-deployment-7cb7b5c689-h8rxv   1/1     Running            0          2m45s
```

Once the MSSQL instance is up and running, run `kubectl get services` and verify the following line exists:
```text
NAME               TYPE           CLUSTER-IP     EXTERNAL-IP    PORT(S)          AGE
mssql-deployment   LoadBalancer   10.0.71.103    20.37.131.79   1433:31162/TCP   3m11s
```

To verify the MSSQL instance is doing its thing, run:
```powershell
sqlcmd -S <EXTERNAL_IP> -U sa -P "mbGJHz11ltChdD9xLnTr"
use msdb
select * from msdb_version
go
```

If all went well, you should get some results back from indicating you're running the latest version of SQL Server (15.0.4102.2 as of the time of this writing).

#### Delete SQL Server Resources
To remove the SQL Server resources, run:

```powershell
kubectl delete service mssql-deployment 
kubectl delete deployment mssql-deployment 
```

### MongoDB
kubectl create namespace mongodb
kubectl apply -f mongodb-crds.yaml
kubectl apply -f mongodb-enterprise.yaml
kubectl create secret generic ops-manager-admin-secret `
  --from-literal=Username="root" `
  --from-literal=Password="password" `
  --from-literal=FirstName="Rooty" `
  --from-literal=LastName="McRoot" `
kubectl apply -f mongodb-ops-manager.yaml -n mongodb
kubectl get om -n mongodb -o yaml -w
kubectl port-forward pods/ops-manager-0 8080:8080 -n mongodb
http://localhost:8080
### Redis

## Deploy
* Open an administrative PowerShell console
* Run the following:
```powershell
# Set a variable for your AKS cluster
$aksCluster = "<your_aks_cluster_name>""
# Login to your Azure account in a browser
az login 
# Login to your Azure container registry
az acr login -n $aksCluster
# Get a token for managing your k8s cluster
az aks get-credentials --resource-group <your_azure_rg> --name $aksCluster
#
tye deploy --interactive
>If you get an error saying `Cannot apply manifests because kubectl is not installed.`, please follow the steps above to install `kubectl`.
```
* When prompted for SQL Server credentials, enter `Server=mssql-deployment,1433;Database=Usidore;MultipleActiveResultSets=true;User ID=sa;Password=mbGJHz11ltChdD9xLnTr`
** The server is a reference to the `msqql-deployment` service
** The port is the port specified in `sqlserver-isntance.k8s.yaml` (1433 in this case)
** The password, for the purporses of this demo, is just the `sa` password set up in the [#SQL Server] section
** This information will all be stored in a new secret created in the k8s cluster
* NEED TO ADD STUFF ABOUT SECRETS
* NEED TO ADD STUFF ABOUT URLS
* NEED TO ADD STUFF ABOUT INGRESS (https://github.com/dotnet/tye/blob/main/docs/recipes/ingress.md)
** Deploy ingress-nginx (y/n):

Once it's complete, you can verify that things were deployed:
```powershell
kubectl get service
```



## Undeploying

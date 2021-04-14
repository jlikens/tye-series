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
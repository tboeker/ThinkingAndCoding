[Cmdletbinding()]
Param(
    [string] $subscription = 'mvd-msdn'
    , [string] $resourceGroupName = 'rg-terraform'  
)

# $containerName='tstate'
Write-Host "subscription: $subscription"
Write-Host "resourceGroupName: $resourceGroupName"

az account set --subscription $subscription

# Create resource group
az group create --name $resourceGroupName --location westeurope

$storageAccountName = "terraform-$subscription".Replace("-", "")
Write-Host "storageAccountName: $storageAccountName"

# Create storage account
az storage account create --resource-group $resourceGroupName `
    --name $storageAccountName `
    --sku Standard_LRS `
    --encryption-services blob

# Get storage account key
$storageAccountKey =$(az storage account keys list --resource-group $resourceGroupName --account-name $storageAccountName --query [0].value -o tsv)
$storageAccountKey

# save to local file
$storageAccountKey | Out-File "$storageAccountName-storageAccountKey.json"

# load from local file
$storageAccountKey = Get-Content -Path "$storageAccountName-storageAccountKey.json"

$serviceConnectioName = "az-tb-thinking-and-coding-terraform-mvd-msdn"
Write-Host "serviceConnectioName: $serviceConnectioName"

$vargroupName = "$subscription-terraform"
Write-Host "vargroupName: $vargroupName"

az pipelines variable-group create `
    --name $vargroupName `
    --authorize true `
    --description 'Variables for Terraform Backend' `
    --organization $org `
    --project $project `
    --variables "BACKEND_SERVICE_CONNECTION=$serviceConnectioName"

$groupId = (az pipelines variable-group list `
        --group-name $vargroupName `
        --organization $org `
        --project $project `
        --query '[0].id' `
        --output tsv)
$groupId


# store in azure devops vars
az pipelines variable-group variable create `
    --group-id $groupId `
    --name BACKEND_RESOURCE_GROUP_NAME `
    --value $resourceGroupName `
    --organization $org `
    --project $project    

az pipelines variable-group variable create `
    --group-id $groupId `
    --name BACKEND_STORAGE_ACCOUNT_NAME `
    --value $storageAccountName `
    --organization $org `
    --project $project    


az pipelines variable-group variable create `
    --group-id $groupId `
    --name BACKEND_STORAGE_ACCOUNT_KEY `
    --value $storageAccountName `
    --secret true `
    --organization $org `
    --project $project    


    
# az pipelines variable-group variable create `
#     --group-id $groupId `
#     --name BACKEND_CONTAINER_NAME `
#     --value $storageAccountName `
#     --organization $org `
#     --project $project    
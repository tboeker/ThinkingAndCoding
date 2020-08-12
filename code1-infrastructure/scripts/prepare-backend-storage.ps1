[Cmdletbinding()]
Param(
        [string] $resourceGroupName = 'rg-terraform'
    ,   [string] $storageAccountName = 'terraformmvdmsdn'
    ,   [string] $containerName = 'x1dev'
)


if ($env:TF_BACKEND_RESOURCE_GROUP_NAME) {
    $resourceGroupName = $env:TF_BACKEND_RESOURCE_GROUP_NAME;
    Write-Host "TF_BACKEND_RESOURCE_GROUP_NAME from Environment"
}
Write-Host "resourceGroupName: $resourceGroupName"

# create resource group
Write-Host "az group create --name $resourceGroupName --location westeurope"
az group create --name $resourceGroupName --location westeurope

if ($env:TF_BACKEND_STORAGE_ACCOUNT_NAME) {
    $storageAccountName = $env:TF_BACKEND_STORAGE_ACCOUNT_NAME;
    Write-Host "TF_BACKEND_STORAGE_ACCOUNT_NAME from Environment"
}
Write-Host "storageAccountName: $storageAccountName"

# create storage account
Write-Host "az storage account create --resource-group $resourceGroupName --name $storageAccountName"
az storage account create --resource-group $resourceGroupName `
    --name $storageAccountName `
    --sku Standard_LRS `
    --encryption-services blob

# get storage account key
$storageAccountKey = $(az storage account keys list --resource-group $resourceGroupName --account-name $storageAccountName --query [0].value -o tsv)
$storageAccountKey

# set storage account key to env
$env:TF_BACKEND_STORAGE_ACCOUNT_KEY=$storageAccountKey
$env:ARM_ACCESS_KEY=$storageAccountKey

# create variable
Write-Host "##vso[task.setvariable variable=BACKEND_STORAGE_ACCOUNT_KEY;isOutput=true]$storageAccountKey"

if ($env:TF_BACKEND_STORAGE_CONTAINER_NAME) {
    $containerName = $env:TF_BACKEND_STORAGE_CONTAINER_NAME;
    Write-Host "TF_BACKEND_STORAGE_CONTAINER_NAME from Environment"
}

Write-Host "az storage container create --name $containerName"
    az storage container create `
    --name $containerName `
    --account-name $storageAccountName `
    --account-key $storageAccountKey


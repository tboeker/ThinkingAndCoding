az login
az account set --subscription 'mvd-msdn'

Push-Location -Path 'C:\repos\ThinkingAndCoding\code1-infrastructure\scripts'

$env:TF_VAR_BACKEND_RESOURCE_GROUP_NAME = 'rg-tb-code1-example-terraform'
$env:TF_VAR_BACKEND_STORAGE_ACCOUNT_NAME = 'tftbcode1example'
$env:TF_VAR_BACKEND_STORAGE_CONTAINER_NAME = 'example'

./prepare-backend-storage.ps1 `
        -containerName $env:TF_VAR_BACKEND_STORAGE_CONTAINER_NAME `
        -resourceGroupName $env:TF_VAR_BACKEND_RESOURCE_GROUP_NAME `
        -storageAccountName $env:TF_VAR_BACKEND_STORAGE_ACCOUNT_NAME

Pop-Location

Push-Location -Path 'C:\repos\ThinkingAndCoding\code1-infrastructure\terraform\env\example'

$env:TF_BACKEND_STORAGE_ACCOUNT_KEY
$env:ARM_ACCESS_KEY

terraform get
terraform init
terraform plan
terraform apply -auto-approve

# cleanup

az group delete --name rg-tb-code1-example-eventstore --yes --no-wait
az group delete --name rg-tb-code1-example-shared --yes --no-wait
az group delete --name rg-tb-code1-example-terraform --yes --no-wait

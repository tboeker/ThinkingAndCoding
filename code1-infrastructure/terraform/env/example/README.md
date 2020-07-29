creating a sample app shared 

```powershell
# login with azure cli
az login
az account set --subscription 'mvd-msdn'

# setting env vars
$env:TF_VAR_EVENTSTORE_YODA_PASSWORD='Password01'


# deploy resources
terraform get
terraform init
terraform apply


# cleanup resources
az group delete --name rg-tb-code1-example-eventstore --yes --no-wait
az group delete --name rg-tb-code1-example-shared --yes --no-wait


```

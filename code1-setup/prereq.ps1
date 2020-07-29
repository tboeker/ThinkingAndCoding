[string] $subscription = 'mvd-msdn'
[string] $subscription = 'tz-msdn'
[string] $subscription = 'mkb-msdn'
[string] $appname = 'code1'
[string] $prefix = 'tb-thinking-and-coding'

# azure devops vars
[string] $org = 'https://dev.azure.com/tboeker'
[string] $project = 'ThinkingAndCoding-Code1'

[string] $githuburl = 'https://github.com/tboeker'

# login and select subscription
az login
az account set --subscription $subscription

$tmp = (az account show)
$account = (ConvertFrom-Json -InputObject  ([System.string]::Concat($tmp)))
$account

# create rbac sp 
$principalname = "az-$prefix-$subscription-$appname"
$principalname
$tmp = (az ad sp create-for-rbac --role="Contributor" --scopes="/subscriptions/$($account.id)" --name $principalname)
$guid = (New-Guid).Guid
$guid
$tmp = az ad sp credential reset --name $principalname --password $guid
$principal = (ConvertFrom-Json -InputObject  ([System.string]::Concat($tmp)))
$principal

# save to local file
$principal | ConvertTo-Json | Out-File "secret-az-$subscription-principal.json"
$account | ConvertTo-Json | Out-File "secret-az-$subscription-account.json"

# load from local file
$principal = Get-Content -Path "secret-az-$subscription-principal.json" | ConvertFrom-Json
$account = Get-Content -Path "secret-az-$subscription-account.json" | ConvertFrom-Json

# save local environment vars
$env:ARM_CLIENT_ID=$principal.appId
$env:ARM_TENANT_ID=$principal.tenant
$env:ARM_SUBSCRIPTION_ID=$account.id
$env:ARM_CLIENT_SECRET=$principal.password
$env:ARM_SUBSCRIPTION=$account.name



# install terraform extension in azuredevops
az devops extension install `
    --extension-name 'custom-terraform-tasks' `
    --publisher-name 'ms-devlabs' `
    --organization $org    

az devops extension install `
    --extension-name 'printAllVariables' `
    --publisher-name 'ShaykiAbramczyk' `
    --organization $org    

# create azure devops project
az devops project create `
    --name $project `
    --open `
    --visibility public `
    --organization $org
                       

# store client secret fot automation
$env:AZURE_DEVOPS_EXT_AZURE_RM_SERVICE_PRINCIPAL_KEY=$principal.password
# create service connection in azure devops
az devops service-endpoint azurerm create `
    --azure-rm-service-principal-id $principal.appId `
    --azure-rm-subscription-id $account.id `
    --azure-rm-subscription-name $account.name `
    --azure-rm-tenant-id $account.tenantId `
    --name $principal.name `
    --organization $org `
    --project $project


# store path for automation
# admin:repo_hook, repo, user
$githubPAT = 'XXX'
$githubPAT = Get-Content -Path "secret-github-pat.txt"
$env:AZURE_DEVOPS_EXT_GITHUB_PAT=$githubPAT

# connect github
az devops service-endpoint github create `
    --github-url $githuburl `
    --name github-tboeker `
    --organization $org `
    --project $project


# store username and password in azure devops variable group

$vargroupName = "$subscription-arm"
Write-Host "vargroupName: $vargroupName"

az pipelines variable-group create `
    --name $vargroupName `
    --authorize true `
    --description 'Variables for Azure Resource Manager Service connection' `
    --organization $org `
    --project $project `
    --variables "ARM_SUBSCRIPTION=$subscription"

$groupId = (az pipelines variable-group list `
        --group-name $vargroupName `
        --organization $org `
        --project $project `
        --query '[0].id' `
        --output tsv)
$groupId


az pipelines variable-group variable create `
    --group-id $groupId `
    --name ARM_SERVICE_CONNECTION `
    --value $principal.name `
    --organization $org `
    --project $project

az pipelines variable-group variable create `
    --group-id $groupId `
    --name ARM_CLIENT_ID `
    --value $principal.appId `
    --organization $org `
    --project $project
  
az pipelines variable-group variable create `
    --group-id $groupId `
    --name ARM_TENANT_ID `
    --value $principal.tenant `
    --organization $org `
    --project $project

az pipelines variable-group variable create `
    --group-id $groupId `
    --name ARM_SUBSCRIPTION_ID `
    --value $account.id `
    --organization $org `
    --project $project

az pipelines variable-group variable create `
    --group-id $groupId `
    --name ARM_CLIENT_SECRET `
    --value $principal.password `
    --organization $org `
    --project $project
  # --secret true `
    

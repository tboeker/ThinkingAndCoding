Push-Location -Path 'C:\repos\ThinkingAndCoding\code1-setup'

Import-Module .\setup-util.psm1 -Force

# login and select subscription
az login

[string] $subscription = 'mvd-msdn'
[string] $subscription = 'tz-msdn'
[string] $subscription = 'mkb-msdn'

[string] $appname = 'code1'
[string] $prefix = 'tb-thinking-and-coding'

# azure devops vars
[string] $org = 'https://dev.azure.com/tboeker'
[string] $project = 'ThinkingAndCoding-Code1'
[string] $githuburl = 'https://github.com/tboeker'

# install global devops extensions
installDevOpsExtensions -org $org

createDevOpsProject -org $org -project $project

# create new account
$account = setSubscription -subscription $subscription

# create new service principal
$principal = createServicePrincipal -subscription $subscription -prefix $prefix -appname $appname

# load from local file
$principal = Get-Content -Path "secret-az-$subscription-principal.json" | ConvertFrom-Json
$account = Get-Content -Path "secret-az-$subscription-account.json" | ConvertFrom-Json
                       
createAzureServiceEndpoint -org $org -project $project -principal $principal -account $account -prefix $prefix -subscription $subscription

createDevOpsArmVariableGroup -org $org -project $project -principal $principal -account $account -prefix $prefix -subscription $subscription


# store path for automation
# admin:repo_hook, repo, user
$githubPAT = 'XXX'
$githubPAT = Get-Content -Path "secret-github-pat.txt"
$githubPAT
createGithubServiceEndpoint -org $org -project $project -name github-tboeker -pat $githubPAT -url $githuburl


$account
$principal


# save local environment vars
$env:ARM_CLIENT_ID=$principal.appId
$env:ARM_TENANT_ID=$principal.tenant
$env:ARM_SUBSCRIPTION_ID=$account.id
$env:ARM_CLIENT_SECRET=$principal.password
$env:ARM_SUBSCRIPTION=$account.name
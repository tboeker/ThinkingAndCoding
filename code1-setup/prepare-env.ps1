Push-Location -Path 'C:\repos\ThinkingAndCoding\code1-setup'

Remove-Module -Name setup-util
Import-Module .\setup-util.psm1 -Force

az login

[string] $appname = 'code1'

# azure devops vars
[string] $org = 'https://dev.azure.com/tboeker'
[string] $project = 'ThinkingAndCoding-Code1'


[string] $envx = 'dev'

$groupId = createEnvVariableGroup -org $org -project $project -envx $envx -appname $appname
$groupId

$eventStoreYodaPassword = 'Password02'
$vname = 'EventStore_YodaPassword'
saveVarInGroup -org $org -project $project -groupId $groupId -name $vname -value $eventStoreYodaPassword

$eventStoreYodaPassword = 'Password0333'
$vname = 'EventStore_YodaPassword2'
saveVarInGroup -org $org -project $project -groupId $groupId -name $vname -value $eventStoreYodaPassword -secret 'true'





# $tmp = (az pipelines variable-group variable list --group-id $groupId --organization $org --project $project)
# $xx = (ConvertFrom-Json -InputObject  ([System.string]::Concat($tmp)))

# az pipelines variable-group variable list --group-id $groupId --organization $org --project $project --query `"[0]==`'EventStore_YodaPassword`"

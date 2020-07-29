function setSubscription() {
    param($subscription)
    
    $tmp =  (az account set --subscription $subscription)

    $tmp = (az account show)
    Write-Host $tmp

    $account = (ConvertFrom-Json -InputObject  ([System.string]::Concat($tmp)))
    # $account

    # save to local file
    $account | ConvertTo-Json | Out-File "secret-az-$subscription-account.json"
    Write-Host $account
    return $account
}



function createServicePrincipal() {
    param($subscription,$prefix,$appname)

    # create rbac sp 
    $principalname = "az-$prefix-$subscription-$appname"
    Write-Host $principalname
    $tmp = (az ad sp create-for-rbac --role="Contributor" --scopes="/subscriptions/$($account.id)" --name $principalname)
    Write-Host $tmp
    
    $guid = (New-Guid).Guid
    Write-Host $guid
    $tmp = az ad sp credential reset --name $principalname --password $guid
    Write-Host $tmp
    $principal = (ConvertFrom-Json -InputObject  ([System.string]::Concat($tmp)))
   
    # save to local file
    $principal | ConvertTo-Json | Out-File "secret-az-$subscription-principal.json"
    Write-Host $principal

    return $principal
}


function installDevOpsExtensions() {
    param($org)

    # install terraform extension in azuredevops
    az devops extension install `
    --extension-name 'custom-terraform-tasks' `
    --publisher-name 'ms-devlabs' `
    --organization $org    

    az devops extension install `
    --extension-name 'printAllVariables' `
    --publisher-name 'ShaykiAbramczyk' `
    --organization $org    

}


function createDevOpsProject() {
    param($org, $project)

    # create azure devops project
    az devops project create `
        --name $project `
        --open `
        --visibility public `
        --organization $org

}

function createAzureServiceEndpoint() {
    param($org, $project, $principal, $account, $prefix, $subscription)
    
    [string] $serviceConnectionName = "az-$prefix-$subscription"

    # store client secret fot automation
    $env:AZURE_DEVOPS_EXT_AZURE_RM_SERVICE_PRINCIPAL_KEY=$principal.password
    # create service connection in azure devops
    az devops service-endpoint azurerm create `
        --azure-rm-service-principal-id $principal.appId `
        --azure-rm-subscription-id $account.id `
        --azure-rm-subscription-name $account.name `
        --azure-rm-tenant-id $account.tenantId `
        --name $serviceConnectionName `
        --organization $org `
        --project $project


}

function createGithubServiceEndpoint() {
    param($org, $project, $pat, $url, $name)
  
    $env:AZURE_DEVOPS_EXT_GITHUB_PAT=$pat

    # connect github
    az devops service-endpoint github create `
        --github-url $url `
        --name $name `
        --organization $org `
        --project $project

}


function createDevOpsArmVariableGroup() {
    # store username and password in azure devops variable group

    param($org, $project, $principal, $account, $prefix, $subscription)
        
    $vargroupName = "$subscription-arm"
    Write-Host "vargroupName: $vargroupName"

    $tmp = az pipelines variable-group create `
        --name $vargroupName `
        --authorize true `
        --description 'Variables for Azure Resource Manager Service connection' `
        --organization $org `
        --project $project `
        --variables "ARM_SUBSCRIPTION=$subscription" `
        --authorize true
    Write-Host $tmp

    $groupId = (az pipelines variable-group list `
            --group-name $vargroupName `
            --organization $org `
            --project $project `
            --query '[0].id' `
            --output tsv)
    Write-Host $groupId


    $tmp = az pipelines variable-group variable create `
        --group-id $groupId `
        --name ARM_SERVICE_CONNECTION `
        --value $principal.name `
        --organization $org `
        --project $project
    Write-Host $tmp

    $tmp = az pipelines variable-group variable create `
        --group-id $groupId `
        --name ARM_CLIENT_ID `
        --value $principal.appId `
        --organization $org `
        --project $project
    Write-Host $tmp    

    $tmp = az pipelines variable-group variable create `
        --group-id $groupId `
        --name ARM_TENANT_ID `
        --value $principal.tenant `
        --organization $org `
        --project $project
    Write-Host $tmp

    $tmp = az pipelines variable-group variable create `
        --group-id $groupId `
        --name ARM_SUBSCRIPTION_ID `
        --value $account.id `
        --organization $org `
        --project $project
    Write-Host $tmp

    $tmp = az pipelines variable-group variable create `
        --group-id $groupId `
        --name ARM_CLIENT_SECRET `
        --value $principal.password `
        --organization $org `
        --project $project
    Write-Host $tmp        
    # --secret true `
    
    return $groupId
}


function createEnvVariableGroup() {

    param($org, $project, $envx, $appname)
        
    $vargroupName = "$appname-$envx"
    Write-Host "vargroupName: $vargroupName"

    $tmp = az pipelines variable-group create `
        --name $vargroupName `
        --authorize true `
        --description 'Variables for App Environment' `
        --organization $org `
        --project $project `
        --variables "EnvironmentShortcut=$envx" `
        --authorize true 
    Write-Host $tmp

    $groupId = (az pipelines variable-group list `
            --group-name $vargroupName `
            --organization $org `
            --project $project `
            --query '[0].id' `
            --output tsv)

    return $groupId

}

function saveVarInGroup() {
    param($org, $project, $groupId, $name, $value)

    $tmp = (az pipelines variable-group variable list --group-id $groupId --organization $org --project $project)
    Write-Host $tmp
    
    $xx = (ConvertFrom-Json -InputObject  ([System.string]::Concat($tmp)))
    $a = $xx[$name]
    
    $isnull = $false
    Write-Host "entry val: $a"

    # $isnull ??= $true
    Write-Host $a ?? "Is null? True!"
    Write-Host "Isnull: $isnull"

    # if ($isnull) {

        $tmp = az pipelines variable-group variable update `
        --group-id $groupId `
        --name $name `
        --value `"$value`" `
        --organization $org `
        --project $project

        Write-Host $tmp

    # } else {

        $tmp = az pipelines variable-group variable create `
        --group-id $groupId `
        --name $name `
        --value `"$value`" `
        --organization $org `
        --project $project

        Write-Host $tmp

    # }
 
    
}
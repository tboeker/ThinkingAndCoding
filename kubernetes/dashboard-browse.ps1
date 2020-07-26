[Cmdletbinding()]
Param(
    [switch] $admin
)



$namespace = 'kubernetes-dashboard'
$user = 'kubernetes-dashboard-token'

if ($admin) {
    $user = 'admin-user'
}


$host.ui.RawUI.WindowTitle = $namespace

kubectl --namespace $namespace describe secret $(kubectl --namespace $namespace get secret | Select-String $user | ForEach-Object { $_ -Split '\s+' } | Select-Object -First 1)

Start-Process 'http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:https/proxy/'

kubectl proxy
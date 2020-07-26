$namespace = 'mongodb1'

$b64 = kubectl get secret --namespace $namespace mongodb -o jsonpath="{.data.mongodb-root-password}"
$p = [Text.Encoding]::Utf8.GetString([Convert]::FromBase64String($b64))
Write-Host $p

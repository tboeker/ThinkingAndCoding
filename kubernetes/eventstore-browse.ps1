$namespace = 'eventstore1'


# connect to mongodb
kubectl port-forward --namespace $namespace svc/eventstore-admin 2113:2113

$url = "http://localhost:8001/api/v1/namespaces/$namespace/services/http:eventstore-admin:http/proxy/"
Write-Host $url
Start-Process $url

kubectl proxy
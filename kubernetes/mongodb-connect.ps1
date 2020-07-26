$namespace = 'mongodb1'

Write-Host "MongoDb port forwarding on port 27017"

# connect to mongodb
kubectl port-forward --namespace $namespace svc/mongodb 27017:27017
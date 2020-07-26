# Source Code



## Local Development

Project URLs

| URL | Service  |
| --- | --- |
| http://localhost:8400 | home 
| http://localhost:8411 | proj1
| http://localhost:8412 | proj2
| http://localhost:8402 | hello
| http://localhost:8401 | health-ui
| http://localhost:8403 | swagger-ui


```powershell
# start docker compose for development services
docker-compose up
```

## Build 
```powershell
# build local images
.\build.ps1

# build and push docker images
.\build.ps1 -pushDockerImages
```

## Deployment

### Docker Compose



```powershell
# start docker compose with apps
docker-compose -f docker-compose.yml -f docker-compose-apps.yml up

# open url 
http://localhost:8400
```

### Kubernetes mit Resouce Files

```powershell

# deploy with kubectl for nohost and path-only
kubectl apply -f ./deploy/path-only/.
# open url
http://localhost/code1/home


# deploy with kubectl for host: code1 and root-path
# hosts: 127.0.0.1 code1
kubectl apply -f ./deploy/host-root/.
# open url
http://code1/home

# deploy with kubectl for host: code1 and basepath: app
# hosts: 127.0.0.1 code1
kubectl apply -f ./deploy/host-and-path/.
# open url
http://code1/app/home


# cleanup namespace
kubectl delete namespace code1
```


### Kubernetes mit Helm


[Helm Charts](./charts/README.md)


## Snippets

```
http://localhost/code1              -> home
http://localhost/code1/health       ->
http://localhost/code1/hello
http://localhost/code1/proj1
http://localhost/code1/proj2
http://localhost/code1/swagger


http://code1/proj1
http://localhost/proj1

# http://kubernetes_master_address/api/v1/namespaces/namespace_name/services/[https:]service_name[:port_name]/proxy


kubectl delete namespace code1

http://localhost/proj1
    -> proj -> /

http://localhost/proj1/healthz
    -> proj -> /healthz


# notepad %windir%\system32\drivers\etc\hosts

127.0.0.1 code1
127.0.0.1 hello.code1
127.0.0.1 home.code1
127.0.0.1 proj1.code1
127.0.0.1 proj2.code1
127.0.0.1 health.code1

http://home.code1
http://hello.code1

http://proj1.code1
http://proj2.code1
http://health.code1



# create namespace
'{ "apiVersion": "v1", "kind": "Namespace", "metadata": { "name": "acode1" }}' | kubectl apply -f -

```


```

dotnet new webapi

dotnet new sln
dotnet new globaljson
dotnet new nugetconfig

dotnet new tool-manifest

dotnet tool install minver-cli
$version = dotnet minver

```
# Code 1 HELM CHARTS

```powershell
# test and debug
helm install mycode1 ./code1 --dry-run --debug

helm install mycode1 ./code1 --dry-run --debug `
    --namespace zcode1 --create-namespace `
    --set basePath=zcode1
```

```powershell
# localhost with basePath

# install
helm install myzcode1 ./code1 --namespace zcode1 --create-namespace --set basePath=zcode1

# upgrade or install
helm upgrade --install myzcode1 ./code1 --namespace zcode1 --create-namespace --set basePath=zcode1

# delete
helm delete myzcode1 --namespace zcode1

# open url
http://localhost/zcode1
```



```powershell
# custom host without basePath

# add hosts to hosts
notepad %windir%\system32\drivers\etc\hosts
127.0.0.1 zcode1root

# debug
helm upgrade --install myzcode1-root ./code1 --debug --dry-run `
    --namespace zcode1-root --create-namespace `
    --set basePath=null `
    --set host=zcode1root

# install
helm install myzcode1-root ./code1 `
    --namespace zcode1-root --create-namespace `
    --set basePath=null `
    --set host=zcode1root

# upgrade or install
helm upgrade --install myzcode1-root ./code1 `
    --namespace zcode1-root --create-namespace `
    --set basePath=null `
    --set host=zcode1root

# delete
helm delete myzcode1-root --namespace zcode1-root

# open url
http://zcode1root
```



```powershell
# custom host mit basePath

# add hosts to hosts
notepad %windir%\system32\drivers\etc\hosts
127.0.0.1 zcode1basepath

# debug
helm install myzcode1-basepath ./code1 --debug --dry-run `
    --namespace zcode1-basepath --create-namespace `
    --set basePath=code1 `
    --set host=zcode1basepath


# install
helm install myzcode1-basepath ./code1  `
    --namespace zcode1-basepath --create-namespace `
    --set basePath=code1 `
    --set host=zcode1basepath

# upgrade or install
helm upgrade --install myzcode1-basepath ./code1 `
    --namespace zcode1-basepath --create-namespace `
    --set basePath=code1 `
    --set host=zcode1basepath

# delete
helm delete myzcode1-basepath --namespace zcode1-basepath

# open url
http://zcode1basepath/code1


```

## SNIPPETS


```powershell
# set chart app version
helm package code1 --app-version "1.2.3" 

```

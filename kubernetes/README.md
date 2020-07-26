# KUBERNETES

choco install docker-desktop --force

## KUBECTL

kubectl config view

kubectl config unset clusters
kubectl config unset contexts
kubectl config unset users

kubectl cluster-info

## DASHBOARD

https://github.com/kubernetes/dashboard/tree/master/aio/deploy/helm-chart/kubernetes-dashboard

helm repo add kubernetes-dashboard https://kubernetes.github.io/dashboard/
helm repo update

kubectl apply -f dashboard-namespace.yaml


http://:8001/api/v1/namespaces/kube-system/services/https:kubernetes-dashboard:/proxy/


## BASH

kubectl apply -f shell-demo.yaml
kubectl get pod shell-demo1


# SCRIPTS

kubectl get pods --namespace $namespace
kubectl get service --namespace $namespace
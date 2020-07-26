
helm repo add kubernetes-dashboard https://kubernetes.github.io/dashboard/
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo add eventstore https://eventstore.github.io/EventStore.Charts

helm repo update

# DASHBOARD

helm upgrade --install kubernetes-dashboard kubernetes-dashboard/kubernetes-dashboard --namespace kubernetes-dashboard --create-namespace --set rbac.clusterReadOnlyRole=true

kubectl apply -f dashboard-admin-user.yaml

kubectl get pods --namespace kubernetes-dashboard
kubectl get service --namespace kubernetes-dashboard

# INGRESS

helm upgrade --install -f ingress-helm-config.yaml nginx-ingress stable/nginx-ingress --namespace nginx-ingress --create-namespace
# --set "controller.service.loadBalancerIP=$publicIp" `
# --set "controller.service.annotations.`"service\.beta\.kubernetes\.io/azure-dns-label-name`"=$dnsName"

kubectl get pods --namespace nginx-ingress
kubectl get service --namespace nginx-ingress

# helm delete nginx-ingress --namespace nginx-ingress

# MONGO DB 

helm upgrade --install mongodb bitnami/mongodb --namespace mongodb1 --create-namespace --set "auth.enabled=false"

kubectl get pods --namespace mongodb1
kubectl get service --namespace mongodb1

# kubectl delete namespace mongodb1

# mongodb.mongodb1.svc.cluster.local


# EVENT STORE  - DEV
helm upgrade --install eventstore eventstore/eventstore --namespace eventstore1 --create-namespace --set "imageTag=release-5.0.8"

# tcp://eventstore.eventstore1.svc.cluster.local:1113


# HEALTH CHECKS

kubectl apply -f https://raw.githubusercontent.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/master/deploy/operator/00-namespace.yaml
kubectl apply -f https://raw.githubusercontent.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/master/deploy/operator/crd/healthcheck-crd.yaml
kubectl apply -f https://raw.githubusercontent.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/master/deploy/operator/01-serviceaccount.yaml
kubectl apply -f https://raw.githubusercontent.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/master/deploy/operator/10-cluster_role.yaml
kubectl apply -f https://raw.githubusercontent.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/master/deploy/operator/11-cluster_role_binding.yaml
kubectl apply -f https://raw.githubusercontent.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/master/deploy/operator/12-operator.yaml
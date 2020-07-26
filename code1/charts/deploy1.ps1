
'{ "apiVersion": "v1", "kind": "Namespace", "metadata": { "name": "acode1" }}' | kubectl apply -f -
helm upgrade --install mycode1 ./code1 --namespace acode1 --set basePath=acode1

# run helm
helm upgrade --install mycode1a ./code1 --namespace acode1 --set basePath=acode1 --dry-run --debug

helm upgrade --install mycode1a ./code1 --namespace acode1 --set basePath=acode1 --debug

helm list --namespace code1a


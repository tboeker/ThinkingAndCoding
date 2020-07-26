Push-Location ./my-setup
helm dependency update
Pop-Location


helm install mysetup ./my-setup --dry-run --debug

helm delete mysetup


helm upgrade --render-subchart-notes --install mysetup-dashbord ./my-setup

helm upgrade --install mysetup-dashboard ./my-setup `
    --render-subchart-notes `
    --namespace kubernetes-dashboard --create-namespace `
    --set "install.dashboard=true"

helm upgrade --install mysetup-dashboard ./my-setup `
    --render-subchart-notes `
    --namespace kubernetes-dashboard `
    --set "install.dashboard=true"

helm delete mysetup-dashboard --namespace kubernetes-dashboard



helm upgrade --install mysetup-cert-manager ./my-setup `
    --render-subchart-notes `
    --namespace cert-manager `
    --set "install.certmanager=true"

# cert-manager

kubectl get pods -A
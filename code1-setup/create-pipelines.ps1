[string] $org = 'https://dev.azure.com/tboeker'
[string] $project = 'ThinkingAndCoding-Code1'

[string] $repository = 'tboeker/ThinkingAndCoding'


az pipelines create `
    --name 'infrastructure-shared' `
    --repository $repository `
    --branch master `
    --repository-type github `
    --yaml-path code1-infrastracture/azure-pipelines-shared.yaml `
    --organization $org `
    --project $project `
    --skip-first-run true
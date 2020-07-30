[string] $org = 'https://dev.azure.com/tboeker'
[string] $project = 'ThinkingAndCoding-Code1'

[string] $repository = 'tboeker/ThinkingAndCoding'


az pipelines create `
    --name 'infrastructure-shared' `
    --repository $repository `
    --branch master `
    --repository-type github `
    --yaml-path code1-infrastructure/azure-pipelines-shared.yml `
    --organization $org `
    --project $project `
    --skip-first-run true



    az pipelines create `
    --name 'infrastructure-env' `
    --repository $repository `
    --branch master `
    --repository-type github `
    --yaml-path code1-infrastructure/azure-pipelines-env.yml `
    --organization $org `
    --project $project `
    --skip-first-run true


    az pipelines create `
    --name 'test' `
    --repository $repository `
    --branch master `
    --repository-type github `
    --yaml-path code1-infrastructure/azure-pipelines-test.yml `
    --organization $org `
    --project $project `
    --skip-first-run true
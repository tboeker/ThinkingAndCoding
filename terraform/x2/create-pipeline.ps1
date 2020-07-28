
$org = 'https://dev.azure.com/tboeker'
$project = 'ThinkingAndCoding'

az pipelines create `
    --name 'terraform-x2' `
    --description 'my first terraform pipeline' `
    --repository tboeker/ThinkingAndCoding `
    --branch master `
    --repository-type github `
    --yaml-path terraform/x2/azure-pipelines.yaml `
    --organization $org `
    --project $project `
    --skip-first-run true
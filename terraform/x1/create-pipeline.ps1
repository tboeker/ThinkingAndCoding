
$org = 'https://dev.azure.com/tboeker'
$project = 'ThinkingAndCoding'

az pipelines create `
    --name 'terraform-x1' `
    --description 'my first terraform pipeline' `
    --repository tboeker/ThinkingAndCoding `
    --branch master `
    --repository-type github `
    --yaml-path terraform/x1/azure-pipelines.yaml `
    --organization $org `
    --project $project
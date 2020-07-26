[Cmdletbinding()]
Param(
    [switch] $skipBuild
    , [switch] $pushDockerImages
  )


dotnet tool restore

$ErrorActionPreference = "Stop";

if (!$skipBuild) {
    dotnet run --project targets -- $args
}

if ($pushDockerImages) {
    dotnet run --project targets -- push-docker-images
}

$ver = dotnet tool run minver
helm package ./charts/code1 --app-version $ver --destination ./artifacts/charts --dependency-update
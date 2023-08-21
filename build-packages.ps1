$output = "./nupkgs"
$version = "0.0.1"
$publishConfig = "./nuget/publish-config.json"
$json = Get-Content $publishConfig -Raw | ConvertFrom-Json

Write-Host $json.packagesPaths

dotnet restore

dotnet build --no-restore --configuration Release

dotnet test --no-build --verbosity normal --configuration Release

foreach ( $path in $json.packagesPaths ){
    dotnet pack $path --output $output --no-build --configuration Release -p:PackageVersion=$version 
}


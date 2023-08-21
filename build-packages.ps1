$output = "./nupkgs"
$version = "0.0.1"
$publishConfig = "./nuget/publish-config.json"

$json = Get-Content $publishConfig -Raw | ConvertFrom-Json

Write-Host "Packages to publish"
Write-Host "--------------------------------------------------------"
Write-Host ($json.packagesPaths -join "`n")

dotnet restore

dotnet build --no-restore --configuration Release

if($LASTEXITCODE -ne 0)
{
    throw "dotnet build failed"
}

dotnet test --no-build --verbosity normal --configuration Release

if($LASTEXITCODE -ne 0)
{
    throw "dotnet test failed"
}

foreach ( $path in $json.packagesPaths ){
    $r = dotnet pack $path --output $output --no-build --configuration Release -p:PackageVersion=$version
}

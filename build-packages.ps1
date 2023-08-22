$output = "./nupkgs"
$testsSln = "./src/tests.sln"
$publishConfig = "./nuget/publish-config.json"
$version = Get-Date -Format "MM.dd.yy.FFFFFF"

if ($null -ne $Env:NugetVersion){
    $version = $Env:NugetVersion
}

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

dotnet test $testsSln --configuration Release

if($LASTEXITCODE -ne 0)
{
    throw "dotnet test failed"
}

foreach ( $path in $json.packagesPaths ){
    dotnet pack $path --output $output --no-build --configuration Release -p:PackageVersion=$version
}

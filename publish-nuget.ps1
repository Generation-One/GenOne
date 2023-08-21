$artifacts = "./nupkgs"
$NugetApiKey = ""
$publishConfig = "./nuget/publish-config.json"

$json = Get-Content $publishConfig -Raw | ConvertFrom-Json

$filenames = (Get-ChildItem -Path $artifacts | Select Name).Name

$count =  $json.packagesPaths.Count
$actualCount = $filenames.Length

if($count -ne $actualCount)
{
    throw "Packages count is $count but actual count is $actualCount"
}

foreach($name in $filenames){
    dotnet nuget push ./nupkgs/$name --source https://api.nuget.org/v3/index.json --api-key $NugetApiKey
}
$artifacts = "./nupkgs"
$NugetApiKey = ""

$filenames = (Get-ChildItem -Path $artifacts | Select Name).Name

foreach($name in $filenames){
    dotnet nuget push ./nupkgs/$name --source https://api.nuget.org/v3/index.json --api-key $NugetApiKey
}
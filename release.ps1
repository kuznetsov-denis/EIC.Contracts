#!/usr/bin/env pwsh

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

$component = Get-Content -Path "component.json" | ConvertFrom-Json
[xml]$xml = Get-Content -Path src/Interface/Interface.csproj
$version = $xml.Project.PropertyGroup.Version

if ($component.version -ne $version) {
    throw "Versions in component.json and Interface.csproj do not match"
}

[xml]$xml2 = Get-Content -Path src/Client/Client.csproj
$version = $xml2.Project.PropertyGroup.Version

if ($component.version -ne $version) {
    throw "Versions in component.json and Client.csproj do not match"
}

# # Build Interface package
# dotnet build src/Interface/Interface.csproj -c Release
# dotnet pack src/Interface/Interface.csproj -c Release -o ../../dist

# # Build Client package
# dotnet build src/Client/Client.csproj -c Release
# dotnet pack src/Client/Client.csproj -c Release -o ../../dist

$packages = (Get-ChildItem -Path "dist/*.$version.nupkg")

foreach ($package in $packages)
{
    $packagePath = $package.FullName
    Write-Output $packagePath

    # Push to nuget repo
    if ($env:NUGET_KEY -ne $null) {
        dotnet nuget push $packagePath -s https://mycompany.com/mvn/api/nuget/cl-nuget-releases -k $env:NUGET_KEY
    } else {
        nuget push $packagePath -Source https://mycompany.com/mvn/api/nuget/cl-nuget-releases
    }
}

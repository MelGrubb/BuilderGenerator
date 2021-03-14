Write-Output "Synchronizing version info"
[xml]$buildProps = Get-Content BuilderGenerator\BuilderGenerator.csproj
$version = $buildProps.Project.PropertyGroup[1].Version
$buildProps.Project.PropertyGroup[1].AssemblyVersion = $version
$buildProps.Project.PropertyGroup[1].FileVersion = $version
$buildProps.Project.PropertyGroup[1].PackageVersion = $version
$buildProps.Save((Resolve-Path "BuilderGenerator\BuilderGenerator.csproj"))

Write-Output "Building version $version"
dotnet build .\BuilderGenerator\BuilderGenerator.csproj --configuration release --verbosity minimal

Write-Output "Publishing"
$path = '.\BuilderGenerator\bin\Release\BuilderGenerator.' + $version + '.nupkg'
dotnet nuget push $path --source 'C:\Projects\NuGet Local Repo'

Uninstall-Package -Id BuilderGenerator -ProjectName Demo
rm C:\Users\Mel\.nuget\packages\buildergenerator\$version -recurse
Install-Package -Id BuilderGenerator -ProjectName Demo
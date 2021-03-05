Write-Output "Synchronizing version info"
[xml]$buildProps = Get-Content Directory.Build.props
$version = $buildProps.Project.PropertyGroup.Version
$buildProps.Project.PropertyGroup.AssemblyVersion = $version
$buildProps.Project.PropertyGroup.FileVersion = $version
$buildProps.Project.PropertyGroup.PackageVersion = $version
$buildProps.Save((Resolve-Path "Directory.Build.props"))

Write-Output "Building version $version"
dotnet build .\BuilderGenerator\BuilderGenerator.csproj --configuration release --verbosity minimal

Write-Output "Publishing"
$path = '.\BuilderGenerator\bin\Release\BuilderGenerator.' + $version + '.nupkg'
dotnet nuget push $path --source 'C:\Projects\NuGet Local Repo'

Uninstall-Package -Id BuilderGenerator -ProjectName Demo
rm C:\Users\Mel\.nuget\packages\buildergenerator\$version -recurse
Install-Package -Id BuilderGenerator -ProjectName Demo
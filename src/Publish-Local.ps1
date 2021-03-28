Write-Output "Synchronizing Assembly Version Info"
[xml]$buildProps = Get-Content BuilderGenerator\BuilderGenerator.csproj
$version = $buildProps.Project.PropertyGroup.Version
$buildProps.Project.PropertyGroup.AssemblyVersion = $version
$buildProps.Project.PropertyGroup.FileVersion = $version
$buildProps.Save((Resolve-Path "BuilderGenerator\BuilderGenerator.csproj"))

Write-Output "Synchronizing Package Version Info"
[xml]$packageProps = Get-Content BuilderGenerator.Package\BuilderGenerator.Package.csproj
$packageProps.Project.PropertyGroup[1].PackageVersion = $version
$packageProps.Save((Resolve-Path "BuilderGenerator.Package\BuilderGenerator.Package.csproj"))

#Write-Output "Building version $version"
#dotnet build .\BuilderGenerator\BuilderGenerator.csproj --configuration release --verbosity minimal

Write-Output "Building Package Version $version"
dotnet build .\BuilderGenerator.Package\BuilderGenerator.Package.csproj --configuration release --verbosity minimal

Write-Output "Publishing package to local NuGet repo"
$path = '.\BuilderGenerator.Package\bin\Release\BuilderGenerator.' + $version + '.nupkg'
dotnet nuget push $path --source 'C:\Projects\NuGet Local Repo'

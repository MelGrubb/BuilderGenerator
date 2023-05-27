[xml]$buildProps = Get-Content BuilderGenerator\BuilderGenerator.csproj
$version = $buildProps.Project.PropertyGroup.Version
$version = "$version".Trim()

Write-Output "Publishing package version '$version' to local NuGet repo"
$path = '.\BuilderGenerator\bin\Debug\BuilderGenerator.' + $version + '.nupkg'
dotnet nuget push $path --source 'C:\NuGet\BuilderGenerator'

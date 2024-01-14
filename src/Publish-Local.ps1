[xml]$buildProps = Get-Content BuilderGenerator\BuilderGenerator.csproj
$version = $buildProps.Project.PropertyGroup.Version

Write-Output "Uninstalling package"
Uninstall-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Sample.NuGet

Write-Output "Deleting local package cache version $version"
if (test-path %userprofile%\.nuget\packages\buildergenerator\$version)
{
    rm %userprofile%\.nuget\packages\buildergenerator\$version -recurse
}

$path = "C:\Projects\NuGet Local Repo\BuilderGenerator.$version.nupkg"
Write-Output "Deleting local NuGet package '$path'"
if (test-path $path)
{
    rm $path
}

Write-Output "Publishing package version '$version' to local NuGet repo"
$path = '.\BuilderGenerator\bin\Debug\BuilderGenerator.' + $version + '.nupkg'
dotnet nuget push $path --source 'C:\NuGet\BuilderGenerator'

Write-Output "Installing new package version"
Install-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Sample.NuGet
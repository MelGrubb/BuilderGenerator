[xml]$buildProps = Get-Content BuilderGenerator\BuilderGenerator.csproj
$version = $buildProps.Project.PropertyGroup.Version

Write-Output "Uninstalling package"
Uninstall-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Test.NuGet

Write-Output "Deleting local package cache version $version"
if (test-path C:\Users\Mel\.nuget\packages\buildergenerator\$version)
{
    rm C:\Users\Mel\.nuget\packages\buildergenerator\$version -recurse
}

$path = "C:\Projects\NuGet Local Repo\BuilderGenerator.$version.nupkg"
Write-Output "Deleting local NuGet package '$path'"
if (test-path $path)
{
    rm $path
}

Write-Output "Publishing new package version"
./Publish-Local.ps1

Write-Output "Installing new package version"
Install-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Test.NuGet
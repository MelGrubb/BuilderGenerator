[xml]$buildProps = Get-Content Directory.Build.props
$version = $buildProps.Project.PropertyGroup.Version
$packageVersion = $buildProps.Project.PropertyGroup.PackageVersion

#Write-Output "Uninstalling package"
#Uninstall-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Sample.NuGet

Write-Output "Deleting local package cache version $packageVersion"
if (test-path %userprofile%\.nuget\packages\buildergenerator\$packageVersion)
{
    rm %userprofile%\.nuget\packages\buildergenerator\$packageVersion -recurse
}

$path = "C:\Projects\NuGet Local Repo\BuilderGenerator.$packageVersion.nupkg"
Write-Output "Deleting local NuGet package '$path'"
if (test-path $path)
{
    rm $path
}

Write-Output "Publishing package version '$packageVersion' to local NuGet repo"
$path = '.\BuilderGenerator\bin\Debug\BuilderGenerator.' + $packageVersion + '.nupkg'
dotnet nuget push $path --source 'C:\Projects\NuGet Local Repo\BuilderGenerator'

#Write-Output "Installing new package version"
#Install-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Sample.NuGet
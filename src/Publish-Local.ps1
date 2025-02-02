[xml]$buildProps = Get-Content Directory.Build.props
$version = $buildProps.Project.PropertyGroup.Version
$packageVersion = $buildProps.Project.PropertyGroup.PackageVersion

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

Write-Output "Updating integration test project references"
dotnet add .\BuilderGenerator.Tests.Integration.Net60.PackageRef\BuilderGenerator.Tests.Integration.Net60.PackageRef.csproj package BuilderGenerator -s 'C:\Projects\NuGet Local Repo\BuilderGenerator' -v $packageVersion
dotnet add .\BuilderGenerator.Tests.Integration.Net70.PackageRef\BuilderGenerator.Tests.Integration.Net70.PackageRef.csproj package BuilderGenerator -s 'C:\Projects\NuGet Local Repo\BuilderGenerator' -v $packageVersion
dotnet add .\BuilderGenerator.Tests.Integration.Net80.PackageRef\BuilderGenerator.Tests.Integration.Net80.PackageRef.csproj package BuilderGenerator -s 'C:\Projects\NuGet Local Repo\BuilderGenerator' -v $packageVersion
dotnet add .\BuilderGenerator.Tests.Integration.Net90.PackageRef\BuilderGenerator.Tests.Integration.Net90.PackageRef.csproj package BuilderGenerator -s 'C:\Projects\NuGet Local Repo\BuilderGenerator' -v $packageVersion

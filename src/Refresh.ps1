Uninstall-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Test.NuGet

if (test-path C:\Users\Mel\.nuget\packages\buildergenerator\$version)
{
    rm C:\Users\Mel\.nuget\packages\buildergenerator\$version -recurse
}

./Publish-Local.ps1

Install-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Test.NuGet
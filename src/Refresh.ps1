./Publish-Local.ps1

Uninstall-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Sample
rm C:\Users\Mel\.nuget\packages\buildergenerator\$version -recurse
Install-Package -Id BuilderGenerator -ProjectName BuilderGenerator.Sample
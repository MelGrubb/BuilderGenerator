[![Nuget](https://img.shields.io/nuget/dt/kaede)](https://www.nuget.org/packages/BuilderGenerator/)
[![GitHub](https://img.shields.io/github/license/am4u/kaede)](https://opensource.org/licenses/MIT)
[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/MelGrubb/BuilderGenerator/ci)](https://github.com/MelGrubb/BuilderGenerator/actions/workflows/ci.yml)
[![GitHub issues](https://img.shields.io/github/issues/melgrubb/buildergenerator)](https://github.com/MelGrubb/BuilderGenerator/issues)

# kaede - an updated version of BuilderGenerator #

Kaede is an updated version of BuilderGenerator - a .NET Source Generator by MelGrubb, that is designed to add "Builders" to your projects. [Builders](https://en.wikipedia.org/wiki/Builder_pattern) are an object creation pattern, similar to the [Object Mother](https://martinfowler.com/bliki/ObjectMother.html) pattern. Object Mothers and Builders are most commonly used to create objects for testing, but they can be used anywhere you want "canned" objects.

For more complete documentation, please see the [documentation site](https://melgrubb.github.io/BuilderGenerator/) or the raw [documentation source](https://github.com/MelGrubb/BuilderGenerator/blob/main/docs/index.md).

## Installation ##

Kaede is installed as an analyzer via NuGet package (https://www.nuget.org/packages/Kaede/). You can find it through the "Manage NuGet Packages" dialog in Visual Studio, or from the command line.

```ps
Install-Package Kaede
```

## Usage ##

After installation, create a partial class to define your builder in. Decorate it with the ```BuilderFor``` attribute, specifying the type of class that the builder is meant to build (e.g. ```[BuilderFor(typeof(Foo))]```. Define any factory and helper methods in this partial class. Meanwhile, another partial class definition will be auto-generated which contains all the "boring" parts such as the backing fields and "with" methods.

## Version History ##
- v1.0
  - Initial fork
  - Updated to include record type support

[![Nuget](https://img.shields.io/nuget/dt/buildergenerator)](https://www.nuget.org/packages/BuilderGenerator/)
[![GitHub](https://img.shields.io/github/license/melgrubb/buildergenerator)](https://opensource.org/licenses/MIT)
[![GitHub issues](https://img.shields.io/github/issues/melgrubb/buildergenerator)](https://github.com/MelGrubb/BuilderGenerator/issues)
[![CI](https://github.com/MelGrubb/BuilderGenerator/actions/workflows/ci.yml/badge.svg)](https://github.com/MelGrubb/BuilderGenerator/actions/workflows/ci.yml)
[![Discord](https://img.shields.io/discord/813785114722697258?logo=discord&logoColor=white)](https://discord.com/channels/813785114722697258/1099524153436012694)

# Builder Generator #

This is a .Net Source Generator designed to add "Builders" to your projects. [Builders](https://en.wikipedia.org/wiki/Builder_pattern) are an object creation pattern, similar to the [Object Mother](https://martinfowler.com/bliki/ObjectMother.html) pattern. Object Mothers and Builders are most commonly used to create objects for testing, but they can be used anywhere you want "canned" objects.

For more complete documentation, please see the [documentation site](https://melgrubb.github.io/BuilderGenerator/) or the raw [documentation source](https://github.com/MelGrubb/BuilderGenerator/blob/main/docs/index.md).

## Installation ##

BuilderGenerator is installed as an analyzer via NuGet package (https://www.nuget.org/packages/BuilderGenerator/). You can find it through the "Manage NuGet Packages" dialog in Visual Studio, or from the command line.

```ps
Install-Package BuilderGenerator
```

## Usage ##

After installation, create a partial class to define your builder in. Decorate it with the ```BuilderFor``` attribute, specifying the type of class that the builder is meant to build (e.g. ```[BuilderFor(typeof(Foo))]```. Define any factory and helper methods in this partial class. Meanwhile, another partial class definition will be auto-generated which contains all the "boring" parts such as the backing fields and "with" methods.

## Version History ##
- v3.0.7
  - Properties marked as Obsolete are ignored by the Builders
  - The "Object" property is now named for the Builder's target class
  - Solved the "Duplicate Definition" problem (41)
  - PostBuildAction replaces PostProcess method

- v2.4.0
    - Test code reorganization
    - Moved WithObject from the base class to the generated builder class
    - Added WithValuesFrom method to shallow clone an example object.

- v2.3.0
    - Major caching and performance improvements
    - Internal code cleanup
    - Conversion of templates to embedded resources

- v2.2.0
  - Changed generated file extension to .g.cs

- v2.0.7
  - Fixed #13, NetStandard2.0 compatibility

- v2.0.6
  - Fixed #12, Generated files now marked with auth-generated header

- v2.0.5
  - Fixed #14, duplicate properties

- v2.0.3
  - Attempting to fix NuGet packaging problems

- v2.0.2
  - Setters for base class properties rendering properly

- v2.0.1
  - Improved error handling

- v2.0.0
  - Updated to .Net 6 and IIncrementalGenerator (See note above about incompatibility with VS2019)
  - Changed usage pattern from marking target classes with attributes to marking partial builder classes

- v1.2
  - Solution reorganization
  - Version number synchronization
  - Automated build pipeline

- v1.0
  - First major release

- v0.5
  - Public beta
  - Working NuGet package
  - Customizable templates

## Roadmap ##

- Read-only collection support in default templates
- Attribute-less generation of partial classes
- Completed documentation
- Unit tests for generation components

## Attributions ##

The BuilderGenerator logo includes [tools](https://thenounproject.com/term/tools/11192) by John Caserta from the Noun Project.

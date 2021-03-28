![Nuget](https://img.shields.io/nuget/dt/buildergenerator)
![GitHub](https://img.shields.io/github/license/melgrubb/buildergenerator)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/MelGrubb/BuilderGenerator/CI%20Workflow)
![GitHub issues](https://img.shields.io/github/issues/melgrubb/buildergenerator)

![Discord](https://img.shields.io/discord/813785114722697258?logo=discord)
![Gitter](https://img.shields.io/gitter/room/melgrubb/buildergenerator?logo=gitter)


# Builder Generator #

This is a .Net Source Generator designed to add "Builders" to your projects. [Builders](https://en.wikipedia.org/wiki/Builder_pattern) are an object creation pattern, similar to the [Object Mother](https://martinfowler.com/bliki/ObjectMother.html) pattern. Object Mothers and Builders are most commonly used to create objects for testing, but they can be used anywhere you want "canned" objects.

For more complete documentation, please see the [documentation site](https://melgrubb.github.io/BuilderGenerator/) or the raw [documentation source](https://github.com/MelGrubb/BuilderGenerator/blob/main/docs/index.md).

## Work In Progress ##

This project and NuGet package are considered beta-phase. It is ready for general testing, but is not yet considered completely done. See the Roadmap section for further details.

## Known Issues ##

As of Visual Studio version 16.9.2, you may see complaints from CodeLens when editing classes decorated with the ```[GenerateBuilder]``` attribute. This is a known issue, and should be fixed in a future release. In the meantime, the builder classes are being properly generated and can be used. It's an annoyance more than anything, and hopefully we won't have to live with it for much longer.

## Installation ##

BuilderGenerator is installed as an analyzer via NuGet package (https://www.nuget.org/packages/BuilderGenerator/). You can find it through the "Manage NuGet Packages" dialog in Visual Studio, or from the command line.

```ps
Install-Package BuilderGenerator
```

## Usage ##

After installation, decorate your classes with the ```GenerateBuilder``` attribute to mark them for generation. Builder classes will be generated in a "Builders" namespace next to the source classes.

## Roadmap ##

- v0.5 - Public beta
  - Working NuGet package
  - Customizable templates

- v1.0 - First major release
  - Automated build pipeline
  - Completed documentation

- v1.1 - Read-only collection support

## Attributions ##

The BuilderGenerator logo includes [tools](https://thenounproject.com/term/tools/11192) by John Caserta from the Noun Project.

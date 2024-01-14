# Contributing

This project welcomes issues and pull requests, although it is an opinionated project. There are many ways to approach this particular problem, and I (Mel Grubb) captured my own preferences when I started this project.

# Building

# Testing

There are currently five test projects.
- BuilderGenerator.Tests.Core: Defines the domain objects used by other test projects.
- BuilderGenerator.Tests.Unit: Directly exercises the builder mechanism to provide fast feedback and verification.
- BuilderGenerator.Tests.Integration.Net60: Uses the builder generator in a more real-world way to create builders for a sample Net 6 library project.
- BuilderGenerator.Tests.Integration.Net70: Same thing, but targeting .Net 7
- BuilderGenerator.Tests.Integration.Net80: Same thing, but targeting .Net 8

# BuilderGenerator #

This is a work in progress.

This NuGet package automates the generation of object builders for testing. It generates the repetitive half of creating builders, leaving only the hand-curated parts and factory methods for you to create as partial classes.

## General Philosophy ##

Builders provide a way to describe desired objects using a fluent syntax. They are commonly used to create test instances of classes for testing purposes, although they can be used to create real-world objects as well, although that is not as common. The majority of the time, test object builders will provide, through the use of static factory methods, a set of well-known or "canned" objects. For instance, an AccountBuilder class may define an "Admin" factory method that returns an administrator account to be used as part of a test. Similarly, it may expose a "Client" method that returns a specific kind of user, in this example a "client".

The fluent interface lets you take a pre-defined object instance and further modify it before it is eventually instantiated by calling the "Build" method.
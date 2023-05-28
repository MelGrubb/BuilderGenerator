# Entities #

This folder contains sample entities for which builders will be created. It's your standard, generic Foo/Bar/Baz hierarchy, with edge cases exercised as described blow.

## Hierarchy ##

Foo contains a read-only collection or Bars.
Bars contains a read-only collection of Bazs.
Baz has no children of its own.

Each layer also contains writable collections of strings in various formats to test the Builder's ability to adapt and choose appropriate properties to handle.

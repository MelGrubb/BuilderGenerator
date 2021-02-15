# BuilderGenerator #

This project contains the implementation of the BuilderGenerator class itself, along with various support classes.

## BuilderGeneratorSyntaxReceiver ##

This class handles the filtering to decide which classes should and should not get a builder.

## TemplateParser ##

This is a utility class to do regex-based search and replace over a string template, looking for all tags in a given format, and replacing them with their corresponding values in an internal dictionary of values. Its main difference is that it only needs to make a single pass over the source string. It is used here as a way to set certain values only once, while allowing others, such as property names, to change on each iteration through a loop.

## Templates ##

The string templates are gathered together here to keep the BuilderGenerator class small and focused. The hope is to eventually pull these templates from the consuming project itself to allow for customization on a per-project basis.

## Extensions ##

Along with the above classes, several extensions are provided to make the BuilderGenerator class easier to read and understand.
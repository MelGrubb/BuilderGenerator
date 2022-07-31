# BuilderGenerator #

This project contains the implementation of the BuilderGenerator class itself, along with various support classes.

## BuilderGenerator ##

This class is the brains of the outfit. It reacts to changes in the target project, looking for classes decorated with the BuilderFor attribute, and generating partial builder classes for them.

## TemplateParser ##

This is a utility class to do regex-based search and replace over a string template, looking for all tags in a given format, and replacing them with their corresponding values in an internal dictionary of values. Its main difference is that it only needs to make a single pass over the source string. It is used here as a way to set certain values only once, while allowing others, such as property names, to change on each iteration through a loop.

## Templates ##

The string templates are gathered together here to keep the BuilderGenerator class small and focused. The hope is to eventually pull these templates from the consuming project itself to allow for customization on a per-project basis. Since the move to .Net 6, the mechanism being used previously no longer works, so I've put this on hold, but hope to return to it.

## Diagnostics ##

Diagnostic report classes to allow the generator to complain about things it doesn't like.
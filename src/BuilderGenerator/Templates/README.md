# Templates

This is still a work in progress, but the idea is to store the templates as properties on an class. These classes can then inherit from previous versions to change only the things they have to. For instance, C# 11 introduced the ability to create attributes with generic parameters. The CSharp11 class inherits the CSharp10 class and overrides the template for the attribute, adding the generic version of the attribute.

Specifying the set of templates to use is still under development, but this is a placeholder to work out what the mechanism might look like when it's finished.
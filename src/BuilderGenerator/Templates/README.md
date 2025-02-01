# Templates

The files in this folder represent the templates used to generate different parts of the builders. They are not compiled, but are simply embedded resources. This makes them easier to edit, with a couple caveats. Certain templates rely on having no blank lines at the end, and depending on how you have your IDE configured, it may try to put a blank line back in at the end. The unit tests will help alert you if this happens. Edit the file outside of the IDE in a plain text editor, and remove the trailing blank line.

## Future Plans

This is still a work in progress, but the idea is to store the templates as properties on a class. These classes can then inherit from previous versions to change only the things they have to for customization.
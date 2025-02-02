# Partial Builders #

This folder contains the hand-written half of any builders. Create static factory methods for each well-known or named object instance you want to create.

## Suggestions ##

In addition to specific factory methods, you should define some general-purpose methods such as:

### Simple/Minimal ###

Creates the simplest, most minimal instance that will pass validation. Only fill in the required fields, and leave everything else at their default values. This can serve as the starting point for other, more specific factory methods. For instance, a minimal Customer may be missing address information, and would have no orders.

### Typical ###

This method should return a "typical" example of the class. For a Customer entity, this might mean that the shipping and billing addresses are filled in, and the Customer has multiple orders in various states.
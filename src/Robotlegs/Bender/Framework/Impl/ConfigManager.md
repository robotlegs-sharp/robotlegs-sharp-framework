# Config Manager

The config manager is the part of the context that manages all objects that are passed into the 'Configure' function call for the Context.

When items are passed into Configure. It is checked with all IMatchers on an object and if it passes, it can then then use the handler function on it.

Config manager adds two IMatcher and Handler functions to it.

* Check to see if the object is a Type. If it is it, after initialization of the context it will reflect the type into IConfig and call configure().

* Check to see if the object is an IConfig. If it is it, after initialization of the context it will call configure().


You can add new IMatchers and Handlers for anything that is passed into configure.
Please be aware, that your IMatchers and IHandlers will not wait for the context to initialize before processing. This is a feature tied into the previous handlers.
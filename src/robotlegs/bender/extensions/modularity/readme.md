# Modularity Extension

## Overview

The modularity extensions wires contexts into a hierarchy based on the context view and allows for inter-modular communication.

## Basic usage

Communication between modules is facilitated by the Module Connector.

Setup to allow sending events from one module to the other:

```c#
//ModuleAConfig.cs

[Inject]
public IModuleConnector moduleConnector {get;set}

moduleConnector.OnDefaultChannel()
	.RelayEvent(WarnModuleBEvent.WARN);
```

Setup to allow reception of events from another module:

```c#
//ModuleBConfig.cs
[Inject]
public IModuleConnector moduleConnector {get;set}

moduleConnector.OnDefaultChannel()
	.ReceiveEvent(WarnModuleBEvent.WARN);
```

Now ModuleB can map commands to the event, or allow mediators to attach listeners to it:

```c#
eventCommandMap.Map(WarnModuleBEvent.WARN)
	.ToCommand(HandleWarningFromModuleACommand);
```

All ModuleA needs to do is dispatch the event:

```c#
eventDispatcher.DispatchEvent(new WarnModuleBEvent(WarnModuleBEvent.WARN);
```

## Named channels

If you want to sandbox the communication between two modules, you can use named channels:

```c#
//ModuleAConfig.cs
moduleConnector.OnChannel("A-and-B")
	.RelayEvent(WarnModuleBEvent.WARN);
```

```c#
//ModuleBConfig.as
moduleConnector.OnChannel("A-and-B")
	.ReceiveEvent(WarnModuleBEvent.WARN);
```


## Requirements

This extension requires the following extensions:

+ ContextViewExtension

## Extension Installation

```c#
_context = new Context()
    .Install(typeof(ContextViewExtension), typeof(ModularityExtension))
    .configure(new ContextView(this));
```

In the example above we provide the instance "this" to use as the Context View. We assume that "this" is a valid context view.

By default the extension will be configured to inherit dependencies from parent contexts and expose dependencies to child contexts. You can change this by supplying parameters to the extension during installation:

```c#
_context = new Context()
    .Install(typeof(ContextViewExtension))
    .Install(new ModularityExtension(true, false))
    .Configure(new ContextView(this));
```

The example context above inherits dependencies from parent contexts but does not expose its own dependencies to child contexts. However, child contexts may still inherit dependencies from this context's parents.


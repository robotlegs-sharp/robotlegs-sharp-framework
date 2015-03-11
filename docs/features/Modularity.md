
Modularity
==========



Disclaimer
----------

The modularity extension isn't installed by default in the MVCSBundle. It is however used in some [platform specific MVCS bundles](../Platforms.md).

Summary
-------

The modularity extension helps you communicate between multiple contexts. It does this by giving you access to share events in/out from different contexts.

Basic Use
---------

The default channel, is a static channel that all context's can use by default. You can connect events and pass them from one global event dispatcher to another's.

Once installed, you can inject the IModuleConnector.

```csharp
[Inject]
public var moduleConnector: IModuleConnector;
```

Say we have two contexts who want to communicate. ContextA wants to send a message to ContextB.

You need to setup which event should go to what context. Here is how we connect a one way event from A to B.

```csharp
// Inside Context A
moduleConnector.OnDefaultChannel().RelayEvent(WarnModuleBEvent.Type.WARN);
```

```csharp
// Inside Context B
moduleConnector.OnDefaultChannel().ReceiveEvent(WarnModuleBEvent.Type.WARN);
```

With this setup, if you dispatch an event of type "WarnModuleBEvent.Type.WARN" in ContextA it will also be heard in ContextB.

```csharp
// Inside Context B
eventCommandMap.Map(WarnModuleBEvent.Type.WARN).ToCommand<HandleWarningFromModuleACommand>());
```

And then finally, you'll need to send the event as normal in Context A

```csharp
// Inside Context A
eventDispatcher.Dispatch(new WarnModuleBEvent(WarnModuleBEvent.Type.WARN));
```

Once this happens, the command from Context B called "HandleWarningFromModuleACommand" should be fired from Context A.

Advanced Use
------------

Of course, this is using the default channel. If you have more contexts and want to seperate out events into different channels. You can name a channel.

```csharp
// Inside Context A
moduleConnector.OnChannel("A-and-B").RelayEvent(WarnModuleBEvent.Type.WARN);
```

```csharp
// Inside Context B
moduleConnector.OnChannel("A-and-B").ReceiveEvent(WarnModuleBEvent.Type.WARN);
```

With this in place, other contexts can use the default channel with the same event without conflicting with this channel.

From here
---------

* [Readme](../../README.md)
	* [A Brief Overview](../ABriefOverview.md)
	* [Features](../Features.md)
		* [Context](./Context.md)
		* [Injector](./Injector.md)
		* [Global Event Dispatcher](./GlobalEventDispatcher.md)
		* [Mediators](./Mediators.md)
		* [Commands](./Commands.md)
		* [Guards](./Guards.md)
		* [Hooks](./Hooks.md)
		* [View Processor](./ViewProcessor.md)
		* [Logger](./Logger.md)
	* [Platforms](../Platforms.md)
	* [Common Problems](../CommonProblems.md)
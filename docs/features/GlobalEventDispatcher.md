
Global Event Dispatcher
=======================

The global event dispatcher is the core messaging system for your application.

This is a singleton so wherever you can inject you can get access to it here

```csharp
[Inject]
public IEventDispatcher dispatcher;
```

It used to designed to communicate primarily between the following:

* Mediators
* Commands
* Models / Services *(and other singletons)*

> *Note:* Mediators communicate with views with their own events / event dispatcher.

Events
------

The global event dispatcher uses IEvents to communicate with each other.

The IEvent interface is very simple and just requires you to have an Enum as a key for what you would like the event to do:

```csharp
﻿using System;

namespace robotlegs.bender.extensions.eventDispatcher.api
{
	public interface IEvent
	{
		Enum type { get; }
	}
}
```

You may want to extend ```Event``` when making your own as it speeds up event creation. You'll need to add a key and any data with the event.


```csharp
public class ExampleEvent : Event
{
	public enum Type
	{
		ACTION_1,
		ACTION_2
		ACTION_3
	}
	
	public ExampleEvent(Type type) : base ( type )
	{
		
	}
}
```

Listening to events
-------------------

You can listen for messages on the global event dispatcher like so:

```csharp
dispatcher.AddEventListener(ExampleEvent.Type.ACTION_1, OnAction1);

void OnAction1()
{
	Console.WriteLine("Action 1 fired");
}
```

If you require the IEvent you can get the data like this:

```csharp
dispatcher.AddEventListener(ExampleEvent.Type.ACTION_1, OnAction1);

void OnAction1(IEvent evt)
{
	Console.WriteLine("Action 1 fired with data: " + evt);
}
```

And if you don't want to cast in your function, you can get your typed event like so:

```csharp
dispatcher.AddEventListener<ExampleEvent>(ExampleEvent.Type.ACTION_1, OnAction1);

void OnAction1(ExampleEvent evt)
{
	Console.WriteLine("Action 1 fired with data: " + evt);
}
```

> *Note:* You can add listener like this too without generics
	> ```dispatcher.AddEventListener(ExampleEvent.Type.ACTION_1, (Action<ExampleEvent>)OnAction1);```

It's best practice to only listen from an event from a Mediator (not a Service or Model). Communicating to Models/Services through the global event dispatcher should be done via [Commands](./Commands.md).



Dispatching events
------------------

If you want to send a message. You can send one by dispatching a new event.

```csharp
dispatcher.Dispatch(new ExampleEvent(ExampleEvent.ACTION_1));
```

When this is dispatched, any command mapped to this event or mediator listening for ExampleEvent.ACTION_1 will be called in the order they were listened to.


Why can't we use regular events?
--------------------------------

Unfortunately, events rely on the subscriber knowing the delegate type. This system allows you to not know the type and the data will be cast by the DynamicInvoke function. 

This way, we can downcast the type to IEvent, get the message key. Send it to the appropriate people and upcast it back to how they expect it.

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
		* [Modularity](./Modularity.md)
	* [Platforms](../Platforms.md)
	* [Writing An Extension](../WritingAnExtension.md)
	* [Common Problems](../CommonProblems.md)
	* [The internals (how it all works)](../TheInternals.md)
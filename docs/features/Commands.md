
Commands
========

So commands are small classes that do things in Robotlegs.

You might be adding a view to your application, changing values in your model, calling a method on a service or even all three.

Let's go ahead and make one

```csharp
public class ExampleCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("Executed");
	}
}
```

That's probably even too basic for a command. Again, like a lot of classes in Robotlegs. The command class doesn't have to implement ICommand. It just needs an execute method.

Mapping an event command
------------------------

With the IEventCommandMap we can map events that when dispatched onto the global event dispatcher will trigger a command. This is usually done (but not limited to) in an IConfig in the setup phase.

```csharp
eventCommandMap.Map(EventClass.Type.ACTION_1).ToCommand<ExampleCommand>();
```

You can also change the Execute method used for the command in the mapping phase

```csharp
eventCommandMap.Map(EventClass.Type.ACTION_1).ToCommand<ExampleCommand>().WithExecuteMethod("CustomMethodName");
```

And you can also use the WithGuards to prevent the command being invoked under certain conditions and add WithHooks to add extra functionality just before the command is executed.

```csharp
eventCommandMap.Map(EventClass.Type.ACTION_1).ToCommand<ExampleCommand>().WithGuards<ExampleGuard>().WithHooks<ExampleHook>();
```

> Tip: Read the [Guards](./Guards.md) and [Hooks](./Hooks.md) documentation for more information


Invoking a command
------------------

So with an event mapped to a command. All you have to do to dispatch the event with the correct type on the global event dispatcher and it will be called.

```csharp
dispatcher.Dispatch(new EventClass(EventClass.Type.ACTION_1));
```

It's good to know that you can also invoke a command with the [DirectCommandMap](../../src/robotlegs/bender/extensions/directCommandMap/readme.md) ~~or if you don't like events with the [Signals Extension](./link) extension instead~~.


Getting data in the command
---------------------------

So like the View when creating the mediator. An injection is made for the event that has invoked the event. So you can pass more data into the command.

Here is an example of the command injecting the event command and using that to populate a model.

```csharp
public class ExampleCommand : ICommand
{
	[Inject]
	public EventClass evt;

	[Inject]
	public ExampleModel model;

	public void Execute()
	{
		model.SetData(evt.customData);
	}
}
```

Downcasting Events
------------------

You can also downcast your event type to IEvent in your command if you want to tie more data event types to the same command.

Say I have ```EventClassA``` and ```EventClassB``` which both extend ```Event```  _(which implements ```IEvent```)_.

If I map the event type down to IEvent in the mapping.

```csharp
eventCommandMap.Map<IEvent>(EventClassA.Type.ACTION_1).ToCommand<MultipleEventCommand>();
eventCommandMap.Map<IEvent>(EventClassB.Type.ACTION_3).ToCommand<MultipleEventCommand>();
```

And dispatch the regular events:

```csharp
dispatcher.Dispatch(new EventClassA(EventClassA.Type.ACTION_1));
dispatcher.Dispatch(new EventClassB(EventClassB.Type.ACTION_3));
```

Then I can get both data types from within the command:

```csharp
public class MultipleEventCommand : ICommand
{
	[Inject]
	public IEvent evt;

	public void Execute()
	{
		if (evt is EventClassA)
		{
			// Get data from A
		}
		else if (evt is EventClassB)
		{
			// Get data from B
		}
	}
}
```

Detaining a command
-------------------

It's possible to make a command live longer than the execute function. This is useful if you want to do so something asynchronous. But before you do, make sure you don't want to wrap this functionality into a service.

You'll need to detain your command or else your command may get picked up by the garbage collector.

```csharp
public class DetainedCommand()
{
	[Inject]
	public IEventDispatcher dispatcher;

	[Inject]
	public ExampleService service;

	[Inject]
	public IContext context;

	public void Execute()
	{
		context.Detain(this);

		service.OnSomethingComplete += HandleDoneSuccess;
		service.OnSomethingFailed += HandleDoneFail;
		service.DoSomethingForLater();
	}

	public void HandleDoneSuccess()
	{
		dispatcher.Dispatch(new ExampleEvent(ExampleEvent.Type.ACTION_1));
		context.Release(this);
	}

	public void HandleDoneFail()
	{
    	Console.WriteLine("Failed");
		context.Release(this);
	}
}
```

The context exposes a small class in called [Pin](./Context.md#Pin) which stores and releases your object in a dictionary where it cannot not be garbage collected.

Invoke Command Directly
-----------------------

If you are using event commands, I would try not to call a command directly. However it is possible and is exposed in the [DirectCommandMap](../../src/robotlegs/bender/extensions/directCommandMap/readme.md).

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
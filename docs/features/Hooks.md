
Hooks
=====

Hooks are also small classes *(see a theme here?)*. That attach to other actions and fire just before their method entry point.

Let's have a look at a hook:

```csharp
public class ExampleHook : IHook
{
	public void Hook()
	{
		Console.WriteLine("This is executing before the action");
	}
}
```

Hmm, not bad. You could argue that it's like another command, and we're using it to chain things.


```csharp
public class ExampleHook : IHook
{
	[Inject]
	public IEventDispatcher dispatcher;

	public void Hook()
	{
		dispatcher.Dispatch(new ExampleEvent(ExampleEvent.ACTION_1));
	}
}
```

Which you could do, but it's a bit more than that. You can use hooks to manipulate commands or it's event if needed.

Say I've added a hook onto my command:

```csharp
eventCommandMap.Map(ExampleEvent.Type.ACTION_1).ToCommand<ExampleCommand>().WithHook<ExampleHook>();
```

I can now inject that event and the command into my hook.

```csharp
public class ExampleHook : IHook
{
	[Inject]
	public ExampleCommand commandAboutToFire;

	[Inject]
	public ExampleEvent evt;

	public void Hook()
	{
		commandAboutToFire.DoSomething();

		evt.data = "changed data";
	}
}
```

You also have hooks with the MediatorMap. These get called just before the mediator has been initialized.

Mapping is very similar to the EventCommandMap:

```csharp
mediatorMap.Map<ExampleView>().ToMediator<ExampleMediator>().WithHooks<ExampleHook>();
```

And injectable into the hook also is the view and mediator classes accessible to you if needed.

```csharp
public class ExampleHook : IHook
{
	[Inject]
	public ExampleView view;

	[Inject]
	public ExampleMediator mediator;

	public void Hook()
	{
		view.Supersize();

		mediator.DoSomethingElseToo();
	}
}
```

And feel free to add as many hooks as you like on either EventCommandMap or MediatorMap.

```csharp
mediatorMap.Map<ExampleView>().ToMediator<ExampleMediator>().WithHooks<Hook1, Hook2, Hook3, Hook4>().WithHooks<EventMoreHook1, EvenMoreHook2>();
```

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
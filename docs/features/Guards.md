
Guards
======

Guards are something that allow other processes to happen.

They are part of the framework of Robotlegs and can to allow Commands to be invoked and Mediators to be created. We found in the original version of Robotlegs, lots of conditions at the beginning of commands.

```csharp
public class ExampleCommand
{
	[Inject]
	public IModel model;

	[Inject]
	public IService service;
	
	public void Execute()
	{
		if (!model.InGameScreen)
			return;
		
		if (!model.HasItem)
			return;

		service.DoSomething();
	}
}
```

Now these conditions can be separated out in the mapping phase and hopefully reduce the number of commands needed for certain conditions.

Here is an example of a Guard

```csharp
public class InGameScreenGuard
{
	[Inject]
	public IModel model;
	
	public bool Approve()
	{
		return !model.InGameScreen;
	}
}
```

Here is how you would map the command to that class:

```csharp
commands.Map(EventClass.Type.USE_ITEM).ToCommand<UseItemCommand>().WithGuards<InGameScreenGuard, HasItemGuard>();
```

You can also Mediate differently depending on approval of guards.

For example, if we were creating a Health view which when you clicked on it, enabled you to add health to your character. It's possible to always mediate it with the 'UpdateHealthMediator' to display the correct health but only mediate it to dispatch and use health when in the game (not the pause menu).

```csharp
mediatorMap.Map<HealthView>().ToMediator<UpdateHealthMediator>();
mediatorMap.Map<HealthView>().ToMediator<UseHealthMediator>().WithGuards<InGameScreenGuard>();
```

Be weary that this only happens upon the registration of the view. So if the InGameScreen boolean wasn't updated before the view was registered. The view wouldn't be mediated twice.


From here
------------

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
	* [The internals (how it all works)](../TheInternals.md)
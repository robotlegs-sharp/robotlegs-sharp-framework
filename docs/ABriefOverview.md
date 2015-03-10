
A Brief Overview
=============

Start up
---------

Typically, when you start a Robotlegs project. You'll need to make a context, and then install some extensions/bundles and your own configurations.

Here is an example of starting up

```csharp
public class Main
{
	private IContext _context;

	public void Main()
	{
		_context = new Context()
				.Install<MVCSBundle>().
				.Configure<MyApplicationConfig>();

		_context.Initialize();
	}
}
```

Here we have installed the core MVCSBundle (which sets up all the packages required for MVCS).

And then once Initialize has been called, it will configure our IConfig class 'MyApplicationConfig'.

Inside our MyApplicationConfig, is where we will set up our application, and tie all our components together
Please note, that on configs, the Injector is run and all [Inject] tags will be supplied it's value.

```csharp
public class MyApplicationConfig : IConfig
{
	[Inject]
	public IInjector injector;

	[Inject]
	public IMediatorMap mediatorMap;

	[Inject]
	public IEventCommandMap commandMap;

	[Inject]
	public IEventDispatcher dispatcher;

	[Inject]
	public IContext context;

	public void Configure()
	{
		injector.Map<IMyModel>().ToSingleton<MyModel>();
		mediatorMap.Map<IMyView>().ToMediator<MyMediator>();
		commandMap.Map(MyEvent.Type.STARTUP).ToCommand<StartupCommand>();

        context.AfterInitializing(StartUp);
	}

	private void StartUp()
    {
		dispatcher.Dispatch(new MyEvent(MyEvent.Type.STARTUP));
    }
}
```

We've added a 'AfterInitializing' handler for the context. So we can be sure to boot up our application with our command once all IConfigs have been configured.

[Read more about the context and setting up](./features/Context.md)

Commands
-------------

Commands are small snippets of code that 'do things' in your application. Splitting your code up like this enables you to re-use your instruction code. And good example of a command might be ```LogoutCommand``` or ```SetHighScoreCommand```

In robotlegs Commands are usually triggered by IEvent's.

An IEvent is a small class that contains an key and data. You can extend the Event class to have an easier starting point for creation of an IEvent.

```csharp
public class MyEvent
{
	public enum Type
	{
		STARTUP,
		DO_SOMETHING,
		DO_SOMETHING_ELSE
	}

	public int extraData;

	public MyEvent(Type type, int extraData) : base (type)
	{
		this.extraData = extraData;
	}
}
```

With the EventCommandMap, you can tie event types to commands like so
```csharp
commandMap.Map(MyEvent.Type.DO_SOMETHING).ToCommand<MyCommand>();
```

This is mapped to the global event dispatcher.  So if you dispatch an event on the global event dispatcher, the command will be executed
```csharp
dispatcher.Dispatch(new MyEvent(MyEvent.Type.DO_SOMETHING));
```
Below is an example command. The injector will process all inject tags in this class.
Also the event class that triggered the command, will be available to be injected into your command if needed.

```csharp
public class MyCommand : ICommand
{
	[Inject]
	public IMyModel model;

	[Inject]
	public MyEvent evt;
	
	pubic void Execute()
	{
		model.SetExtraData(evt.extraData);
	}
}
```

[Read more about commands](./features/Commands.md)



Mediators
------------

Mediators are classes that tied to View components. The mediator is a bridge that allows you view to communicate to the rest of the application (usually via the global event dispatcher).

This enables the functionality to the rest of the app from your view to be removed if needed. And enables you to re-use mediation code to tie to new views. Enabling you to re-use the mediators for the same functionality but a different look.

You can easily extend the Mediator base type to create a mediator You can also use the IMediator class implementation if you don't want the global event dispatcher, or the AddViewListener or AddContextListener methods.

A mediator is ready to be used after the ```Initialize``` method has been called.
When the view has been destroyed, the ```Destroy``` method of the mediator will be called for you to cleanup.

Mediators can be tied to interfaces. Making it very easy for views to be managed by smaller and more re-usable mediators.

```csharp
mediatorMap.Map<IMyView>().ToMediator<MyMediator>();
```

You can also tie them to concrete classes rather than interfaces
```csharp
mediatorMap.Map<SimpleView>().ToMediator<SimpleMediator>();
```

Here is an example of a mediator:

```csharp
public MyMediator : Mediator
{
	[Inject]
	public IMyView view;

	public void Initialize()
	{
		AddViewListener(MyEvent.Type.DO_SOMETHING, Dispatch);
		AddContextListener(MyEvent.Type.LISTEN, OnListenFromGlobalEventBus);
	}

	public void Destroy()
	{
		// Here you should cleanup anything you setup
		// (AddViewListener and AddContextListener methods are cleaned up for you)
	}

	private void OnListenFromGlobalEventBus()
	{
		// Inform the view
		view.HeardEvent();

		// Do something else
		Dispatch(new MyEvent(MyEvent.Type.DO_SOMETHING_ELSE));
	}
}
```

Please note, the AddViewListener method assumes your view is a IEventDispatcher or has a property called dispatcher that is an IEventDispatcher for easy communication.

Feel free to use your own C# events to communicate to your view, but remember to clean up everything in the Destroy method.

[Read more about mediators](./features/Mediators.md)

Models
--------

Models are classes (usually mapped as singletons) that manage access to data within your app.
You can get your models, from within a command. Or dispatch an event from the model with data via the global event dispatcher for everyone to hear.

You would use the injector to map your model like this:

```
injector.Map<IMyModel>().ToSingleton<MyService>();
```

Here is an example of basic model that stores some data.

```csharp
public class MyModel : IModel
{
	public int Data
	{
		get { return _data; }
	}

	private int _data;

	public void SetData(int data)
	{
		_data = data;
	}
}
```

[Read more about injector mapping rules](./features/Injector.md)

Services
---------

Services are like Models, but instead of managing date or state, they manage access to remote services.

You would use the injector to map your services
```csharp
injector.Map<IMyService>().ToSingleton<MyService>();
```

Here is an example of  a simple service that dispatches back to the global event dispatcher

```csharp
public MyService : IMyService
{
	[Inject]
	public IEventDispatcher dispatcher;

	public void LoadSomething()
	{
		// Thread, you're very tired...
		System.Threading.Thread.Sleep(500);
		LoadSomethingComplete();
	}

	private void LoadSomethingComplete()
	{
		dispatcher.Dispatch(new MyServiceEvent(MyServiceEvent.Type.LOAD_SUCCESS));
	}
}
```

[Read more about injector mapping rules](./features/Injector.md)

-------------------------

From here
------------

* [Readme](../README.md)
	* [A Brief Overview](./ABriefOverview.md)
	* [Features](./Features.md)
	* [Platforms](./Platforms.md)
	* [Common Problems](./CommonProblems.md)
	* [The internals (how it all works)](./TheInternals.md)
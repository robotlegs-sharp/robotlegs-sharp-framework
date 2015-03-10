
Mediators
=========

Mediators are coupled with views giving them the ability to communicate with the rest of the application.

The view's have their mediators assigned to them in the setup phase of Robotlegs.

Basic Mapping
-------

You can map the views in a few different ways.

```csharp
mediatorMap.Map<ExampleView>().ToMediator<ExampleMediator>();
```

This line of code, will assign an ExampleMediator to be created whenever an ExampleView is registered. This is also the case for a View that extends ExampleView.

Of course, this is good. But it would be better if you could Map based on an interface. Mediators have to be a class (that needs to be created), but the view could be an interface.

For example

```csharp
mediatorMap.Map<IScoreView>().ToMediator<ScoreMediator>();
```

This means, any view that implements IScoreView will have the score mediator applied to it. That's pretty cool and very easy to start re-using lots of mediators. But the fun doesn't stop there...

Mapping With IMatcher
-----------------

The mediator map also allows TypeFilter/NamespaceFilter which is created with the TypeMatcher/NamespaceMatcher classes. Enabling you to truly customise what your mediators will be attached to.

### Type Matcher

With the TypeMatcher, It's possible to Mediate based on many type conditions.

```csharp
mediatorMap.Map(new TypeMatcher().AllOf(typeof(A), typeof(B)).NoneOf(typeof(C )).AnyOf(typeof(D), typeof(E))).ToMediator<ExampleMediator>();
```

This means, it will only mediate any classes that are **both** A & B, **not** a C and **either** a D or E.

The rules are as follows:

1. AllOf() - The type needs to **satisfy all** these classes provied in this function before being mediated.
2. NoneOf() - Any types provided in this method, will **not be mediated**
3. AnyOf() - **Any types** provided in this method will be mediated after satisfying the above two rules

### Namespace Matcher

The namespace matcher allows you to mediate based on namespace.
With similar modifiers to the Type Matcher. The namespace matcher takes strings, and will search if the package starts with that string.

```csharp
mediatorMap.Map(new NamespaceMatcher().Require("example.a").NoneOf("example.a.none").ToMediator<ExampleMediator>();
```

1. Require() - A single namespace string that **must** always be satisfied to be mediated
2. NoneOf()- Namespaces that **will never** be mediated
3. AnyOf() - **Any of these namespaces** that will be mediated after satisfying the above two rules

Registering your view
---------------------

The View Notifier is the static *(yes, I know)* class that informs the appropriate context that your view is ready to be processed.

Your view needs to be an IView in our framework as we need to know when the view has been destroyed. Depending on your platform, there might be a View class you can use to help you extend from.

Here is an example of a View that registers in the constructor and has a method for registering the view.
It's important that this event is called, otherwise your mediator or view processor will not get destroyed.

```csharp
public class ExampleView : IView
{
	public event Action<IView> RemoveView;
	
	public ExampleView()
	{
		ViewNotifier.RegisterView(this, this.GetType());
	}

	public void Cleanup ()
	{
		if (RemoveView != null)
			RemoveView(this);
	}
}
```

That's the most basic view that will get registered.

Let's go a bit further and add some more functionality. Let's add a user click which will call an action, we will also listen for a different action from the context, and will display this to the user.

```csharp
public class ExampleView : IView
{
	public IEventDispatcher dispatcher = new EventDispatcher();

	public event Action<IView> RemoveView;
	
	public ExampleView()
	{
		ViewNotifier.RegisterView(this, this.GetType());
	}
	
	public void ShowPopup(string message)
	{
		// Imagine popup code here...
	}

	public void Cleanup ()
	{
		if (RemoveView != null)
			RemoveView(this);
	}

	private void UserPress()
	{
		dispatcher.Dispatch(new ExampleEvent(ExampleEvent.Type.ACTION_1));
	}
}
```

For communication out to our mediator, I have used a new EventDispatcher (not the global) for the AddViewListener method in the mediator class later. 

> Note: I don't have to use the EventDispatcher, but can dispatch regular C# events also to communicate with my mediator



Which Mediator Base?
-------------

Now that your view is ready to be registered, you've got your mapping setup. You need to make a mediator.

I would recommend extending the Mediator class. But first let's implement the more basic IMediator. We will use it to pass up the user click event from the view to the global dispatcher. And then we'll listen on the global dispatcher for another event and show a popup on our view.

We also have the ability to [Inject] our view of the type it was mapped to from the Mediator Map.

```csharp
public class ExampleMediator : IMediator
{
	[Inject]
	public IEventDispatcher dispatcher;

	[Inject]
	public ExampleView view;

	public void Initialize()
	{
		// Pass up user press event from view to global event dispatcher
		view.AddEventListener(ExampleEvent.Type.ACTION_1, dispatcher.Dispatch);

		// Listen for ACTION_2 on the global event dispatcher and show a popup
		dispatcher.AddEventListener(ExampleEvent.Type.ACTION_2, HandleAction2);
	}

	private void HandleAction2()
	{
		view.ShowPopup("Action 2 just happened.");
	}

	public void Destroy()
	{
		// Make sure I clean up
		view.RemoveEventListener(ExampleEvent.Type.ACTION_1, dispatcher.Dispatch);

		dispatcher.RemoveEventListener(ExampleEvent.Type.ACTION_2, HandleAction2);
	}
}
```

Now, that was relatively painless. If you'd like to know how to get data when listening to your event. Look at the [Listening To Events](./GlobalEventDispatcher.md#listening-to-events) section.

Although we spent a bit of time making sure we cleaned up the event listeners in the destroy. If we extend Mediator we don't have to do that.

```csharp
public class ExampleMediator : Mediator
{
	[Inject]
	public ExampleView view;

	override public void Initialize()
	{
		// Pass up user press event from view to global event dispatcher
		AddViewListener(ExampleEvent.Type.ACTION_1, Dispatch);

		// Listen for ACTION_2 on the global event dispatcher and show a popup
		AddContextListener(ExampleEvent.Type.ACTION_2, HandleAction2);
	}

	private void HandleAction2()
	{
		view.ShowPopup("Action 2 just happened.");
	}
}
```

So notice how we are not cleaning up any code anymore. Anything passed into the convenience methods of 'AddViewListener' or 'AddContextListener' are stored and then removed upon the destroy of the mediator.

> Note: In this example, the AddViewListener will only work if the View class has a property called 'dispatcher' of type IEventDispatcher or if the View class _is_ an IEventDispatcher.

Finally, you don't have to implement IMediator or extend Mediator at all. The Mediation will call a set of methods if avaliable. It will call these when initializing:

* PreInitialize();
* Initialize();
* PostInitialize();

And the following methods when destroying:

* PreDestroy();
* Destroy();
* PostDestroy();

And if you have a property called 'viewComponent' it will populate it with the view that is associated with it regardless of type.


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
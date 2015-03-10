
Unity
=====

Getting Started
---------------

To start using the Unity specific extensions. Instead of installing the MVCSBundle install the Unity specific one:

#### For multiple contexts

```csharp
context = new Context()
	.Install<UnityMVCSBundle>()
    .Configure(new ContextView(transform));
```

The context will initialize automatically for you when a Script on the ContextView has initialized and will destory when your context view has been destoyed.

Now by design, the View's are only mediated/processed when it is parented to a context. So in this heirchy:

```
- View A
- Context
	- View B
	- View C
```

View A will not get processed/mediated (whereas, View B and View C will).

If you want your context to map **all remaining views** that are not parented to a context. Add the FallbackContainerConfig to that Context.

```csharp
context = new Context()
	.Install<UnityMVCSBundle>()
    .Configure<FallbackContainerConfig>()
    .Configure(new ContextView(transform));
```

#### Views

So, now you have access to the **View** and **EventView** clases, which are simple MonoBehaviours implmentations of IView and IEventView for you to start from.

```csharp
public class ExampleView : View
{
	override protected void Start()
    {
		// Before view has been processed
	    base.Start();
		// After view has been processed
    }

	override protected void OnDestroy()
    {
		base.OnDestroy();
    }
}
```

> HINT: If you ever use Start() or OnDestroy(), be sure to base.Start() and base.OnDestroy or you view will not get processed or cleaned up

The EventView class gives you quick access to your own dispatcher to be used with the Mediator class.

```csharp
public class ExampleView : EventView
{
	public void SendMessageForMediator()
    {
		dispatcher.Dispatch(new ExampleEvent(ExampleEvent.Type.JUST_TESTING));
	}
}
```

#### Multiple Contexts

Now, with the IParentFinder implementation you can handle multiple contexts.

```
- View A
- Context1
	- View B
	- View C
- Context2
	- View D
    - View E
	- Context 3
    	- View F
```

When your views register. It will now try and find the closest parent. If it has a mapping, it will get processed by that context. If it doesn't it will get processed by it's parent context.

- View A - Is never processed
- View B - Processed by Context 1
- View C - Processed by Context 1
- View D - Processed by Context 2
- View E - Processed by Context 2
- View F - Processed by Context 3 if mapping found. If it has no mapping it's Processed by Context 2

Each context doesn't share their global IEventDispatcher by default. See the [Modularity](../features/Modularity.md) section on how to communicate between contexts.

Editor Scripts
--------------

To help with development. One of the really useful things for debugging in the StrangeIOC framework was adding your Mediators as Components. Although I disagree that mediators *have* to be components, having the visual cue that your item had been mediated is **very** useful!

#### Mediator Attach

Tied as a Mediator Manager class. Unity will add a tiny component to your view indicating that it has been Mediated. It will remove it from view when it is unmediated.

> Note: This script is **NOT** the mediator. Removing this script will just remove your visual cue of it being mediated.

#### Unity Singletons

We've added Unity Singletons which adds to your ContextView. Visually, you can see what singletons have been mapped at the point. Note that by default, your **AsSingleton** and **ToSingleton** do not instantiate by default until it is first pulled out of the injector. This class will help you visualise this.

Bundle Differences
------------------

We've removed the Console Logging Extension, and have added the Debug Logging Extension so the logger logs out to the correct console.

We've added UnityStageCrawler, which takes the contextview and searches through it's components in children after intialization to process any views that might have already awoken.

We've added a UnityParentFinder, which enables the ViewManager know who is parented to whom.

We've included the Modularity Extension.

We've added a UnityStateWatcher, which moniters the context view to know when it has initalized or been destroyed.

We've added View and EventView classes, to easily extend and to be Mediated.

We've removed the FallbackContainerConfig, and replaced it with the ContextViewListenerConfig which adds the ContextView as the container (and forces user to parent their view's to their ContextView)

From here
------------

* [Readme](../../README.md)
	* [A Brief Overview](../ABriefOverview.md)
	* [Features](../Features.md)
	* [Platforms](../Platforms.md)
		* [Unity](./Unity.md)
		* [Adding A Platform](./AddingAPlatform.md)
	* [Common Problems](../CommonProblems.md)
	* [The internals (how it all works)](../TheInternals.md)
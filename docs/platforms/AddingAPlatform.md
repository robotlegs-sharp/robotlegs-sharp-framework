
Adding A Platform
=================

So there are a few things that we would reccomend adding to a platform (not just new cool features).

Parent Finder
-------------

One of which, is a **ParentFinder** class. This allows our **ContainerRegistry** and the **ViewManager** to know who is a parent of whom. It's interface is as follows:

```csharp
bool Contains (object parentContainer, object childContainer);
object FindParent (object childView, List<ContainerBinding> containers);
```

You'll need to cast the object as the type you are expecting the user to add as View's and Containers (ContextView's) and then satisfy the interface.

In doing this, the communication between Context's and adding View's will work, be sure to add the 'ContextViewListenerConfig' to your bundle.

Stage Sync
----------

As the ContextView is tied to a Context. The View's lifecycle can be syncronised with the Contexts. The **StageSyncExtension**'s job should be initialize and destroy the Context along with the View, and possibly even suspended/resumed if applicable.

Stage Crawler
-------------

It's reccomended to add a **StageCrawlerExtension**. This class functionality searches for View's that need to be processed/mediated that may have been missed before initialization.

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
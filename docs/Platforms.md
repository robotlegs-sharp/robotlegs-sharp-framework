Platforms
=========

Overview
--------

The original Robotlegs framework was dependant on it's platform for view management.

We've had to cut out some view management features in the vanilla bundle, and make it more expandable so you can add a platform.

The main benefits of having platform specific MVCS bundles are:

* Stage Sync, The context's life to by synced with with a view element and automatically call Initialize and Destroy methods.
* View Manager / Container Registy - The ability to have Root containers/views that define which context the view belongs to and find the rules of mediation / viewprocessor.
* Stage Crawler - To look through all view objects and attempt to mediate/process them upon the Context's initialization, incase they were alive before any rules had been assigned.
* Child Context - An automatic parental hierarchy allowing parents to give injection rules to children.

You can also add more features to each platform if required by adding your own Extensions if appropriate.

Avaliable Platforms
-------------------

* [Unity](./platforms/Unity.md)

Adding your own platform
------------------------

We'd reccomend adding some of the features avalible when adding new platforms. Please see [this guide](./platforms/AddingAPlatform.md) to get started.

The Vanillla MVCSBundle
-----------------------

Without platform specific code, the orignal bundle comes packed with MVCS allowing for one context.

**ConsoleLoggingExtension** - Gets our logging to log out to the console

**VigilanceExtension** - Converts logger.Warn() into Errors because we strive for perfection

**InjectableLoggerExtension** - Allows you to inject the logger for easy logging

**EventDispatcherExtension** - Makes our single message channel to communicate

**DirectCommandMapExtension** - Allows you to call command directly without events

**EventCommandMapExtension** - Ties events on the main channel to commands to be invoked

**LocalEventMapExtension** - Gives us local event map to we can easily remove lots of events listeners in the Mediator class

**ViewManagerExtension** - Allows us to call processes on views that get registered (MediatorMap  / ViewProcessorMap)

**MediatorMapExtension** - Allows us to tie multiple meditors to views

**ViewProcessorMapExtension** - Gives the user the ability to do anything else to views

**FallbackContainerConfig** - Sets up all views in the ViewManager to be processed by this config (allows for only one context)

If you want to find out more, check out [the internals](./TheInternals.md) page for more reference.

From here
------------

* [Readme](../README.md)
	* [A Brief Overview](./ABriefOverview.md)
	* [Features](./Features.md)
	* [Platforms](./Platforms.md)
	* [Common Problems](./CommonProblems.md)
	* [The internals (how it all works)](./TheInternals.md)
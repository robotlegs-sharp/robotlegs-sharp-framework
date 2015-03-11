
Features
========

#### [Context](./features/Context.md)

The core framework for Robotlegs is the context. It's is a simple item designed for installing other code via the Install and Configure methods.
The context has a lifecycle phase, as in it can be initialized once, paused and resumed multiple times and finally destroyed.
If you have such a large application you can even add multiple contexts for different sections of the application and make the context communicate with each other on a different event dispatcher.

#### [Injector](./features/Injector.md)

Behind the context, supplying dependencies throughout the extensions is the Injector.
It's got lot of nifty features on how you want to provide values.

#### [Global Event Dispatcher](./features/GlobalEventDispatcher.md)

A messaging system of the Robotlegs framework. This provides data to mediators, models, services and even invokes commands with data.

#### [Mediators](./features/Mediators.md)

The view management system of Robotlegs with it's TypeMatcher and NamespaceMatcher allows you to mediate based on lots of conditions.

#### [Commands](./features/Commands.md)

The event command system allowing you to 'do' things in your application. Add Views, call services, update models, call parsers. If you can think of something re-usable to do, you should do it in these classes.

#### [Guards](./features/Guards.md)

Guards are tiny classes that simply Approve allowing other code to execute. We use it in the framework to allow Commands to be fired, Mediators to be created, and View Processors to be handled.

#### [Hooks](./features/Hooks.md)

Hooks are tiny classes that you can attach to the end of other executing code.
Like Guards, they are used in the framework before a Command has been fired, Mediator has been Initialized and View Processors have been handled.

#### [View Processor](./features/ViewProcessor.md)

The view processor allows you to perform an action onto registered views. It uses the same IMatching system the Mediator Map uses so you can easily filter the action on certain views.

#### [Logger](./features/Logger.md)

The logger is a simple tool to help you debug. It's logs with four different LogLevels and has the ability to tie in to your own systems.
You should be using the logger to debug when you write your extensions as it's useful for other people to see what's going on if things are not working.

#### [Modularity](./features/Modularity.md)

The modularity enables you to setup communication between each context's global EventDispatcher.

From here
------------

* [Readme](../README.md)
	* [A Brief Overview](./ABriefOverview.md)
	* [Features](./Features.md)
	* [Platforms](./Platforms.md)
	* [Writing An Extension](./WritingAnExtension.md)
	* [Common Problems](./CommonProblems.md)
	* [The internals (how it all works)](./TheInternals.md)
The Internals
=============

Framework
---------

The framework of Robotlegs 2 is a lot smaller that it's predecessor. It's aim is to provide a small and lightweight base for installing other plugin base code with a few core features to initialize expandability.

The framework is the Context class which incorperates and combines a set of smaller functional classes.

#### Injector

The injector is at the heart of Robotlegs. It's a dependancy provider allowing you to provide different values for Interfaces or Classes. This enables us to easily re-use code.

#### Extension

The **Extension Installer** give the classes a reference to the context. Enabling them to have complete access to the framework.

The **Config Manager** allows user to configure based on pre-mapped values in the injector.

#### Utilities

The **Pin** keeps reference of object instances preventing them being garbage collected.

The **Guards** are a simple class allowing/prevening things to happen.

The **Hooks** are simple actions that happen in order.

* Context
	* Injector
	* Pin
	* Lifecycle
		* Lifecycle State
	* Log Manager
		* Logger
	* Config Manager
		* Object Processor
	* Extension Installer
	* Guards
	* Hooks
	* Children

Extensions
----------

There are a lot of extensions avalible and installed for you in the MVCSBundle, and a lot of them work together to function correctly.

Lets talk about how some of them work.

#### View Management

View management, is something that is happening in the background listening to Views that get registered. And then giving that to the correct context to be processed.

The **Container Registry** has a static dictionary of all containers. A container is the root of all views (often a context view). The Container Registry knows the relation of each container (if they are parents, siblings or children). Because this dictionary is static, it persists throughout multiple contexts.

The **View Manager** knows about some containers. You have one assigned to a single context. Usually you will only have one container (the context view) but can have multiple root containers. When you add it here, it gets added to the container registry.

The **Stage Crawler** extension uses the ViewManager's containers, and scans all children for View's to register. This is done incase the view's registered early and were not processed.

The **Context View** just contains your root view, it assigns the view as a container to the View Manager.

#### Expanding The Context

The **Stage Sync** extension uses the ContextView's view as the object to sync the Context's lifecycle extension with.


The **Viligence** extension changes the ```logger.Warn("Message") ``` and ```logger.Error("message")``` from the context into execeptions that are thrown.

#### Advanced Mapping

We have added the **IMatcher** classes as for checking types before being processed. This is used to help in the EventCommand, ViewProcessor and MediatorMap.

#### Commands

The **CommandCenter** classes is some classes for other extensions to use. The command center give you the command executor. Which can execute commands with a payload. This is great, but as a robotlegs user, it is not part of an extensions. Nothing is mapped yet.

The **DirectCommandMap** is the simplist implementation of the CommamndCenter library. It mapps a class to give you a simple set of API to execute your commands with payloads.

The **EventCommandMap** is the event based command center. This stores event types and listenes for them on the event dispatcher. Then when it hears the event it will add a injection rule for the event class and then fire the command.

#### View Handling

The **MediatorMap** adds a delegate to the ViewManager. This gives us the view when registered. The mediator map, attaches mediators and associates them with this view.
The Mediator class uses the **LocalEventMap** a class to help manage listeners you've added and remove them all upon disposal.

The **ViewProcessorMap**

#### Modularity

The **Modularity Extension**

A fairly detailed overview

* [Command Center](../src/robotlegs/bender/extensions/commandCenter/readme.md)
* [Context View](../src/robotlegs/bender/extensions/contextView/readme.md)
	* Stage Sync
	* Context View Listener Config
* [Direct Command Map](../src/robotlegs/bender/extensions/directCommandMap/readme.md)
* [Enhanced Logging](../src/robotlegs/bender/extensions/enhancedLogging/readme.md)
	* [Console Logging](../src/robotlegs/bender/extensions/enhancedLogging/readme.md#TraceLoggerExtension)
	* [Injectable Logger](../src/robotlegs/bender/extensions/enhancedLogging/readme.md#InjectableLoggerExtension)
	* [Injector Activity Logging](../src/robotlegs/bender/extensions/enhancedLogging/readme.md#InjectorActivityLoggingExtension)
* [Event Command Map](../src/robotlegs/bender/extensions/eventCommandMap/readme.md)
* [Event Dispatcher](../src/robotlegs/bender/extensions/eventDispatcher/readme.md)
* [Local Event Map](../src/robotlegs/bender/extensions/localEventMap/readme.md)
* [Matching](../src/robotlegs/bender/extensions/matching/readme.md)
	* InstanceOf Matcher
	* [Type Matcher](../src/robotlegs/bender/extensions/matching/readme.md#TypeMatcher Usage)
	* [Namespace Matcher](../src/robotlegs/bender/extensions/matching/readme.md#NamespaceMatcher Usage)
* [Mediator Map](../src/robotlegs/bender/extensions/mediatorMap/readme.md)
* [Modularity Extension](../src/robotlegs/bender/extensions/modularity/readme.md)
	* Module Connector
* [View Manager](../src/robotlegs/bender/extensions/viewManager/readme.md)
	* Container Registry
	* Parent Finder
	* View Notifier
* [View Processor Map](../src/robotlegs/bender/extensions/viewProcessorMap/readme.md)
* [Vigilance](../src/robotlegs/bender/extensions/vigilance/readme.md)

Including Platforms
-------------------

What you need to do to include a platform, and how it ties in


From here
------------

* [Readme](../README.md)
	* [A Brief Overview](./ABriefOverview.md)
	* [Features](./Features.md)
	* [Platforms](./Platforms.md)
	* [Common Problems](./CommonProblems.md)
	* [The internals (how it all works)](./TheInternals.md)
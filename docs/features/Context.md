
Context
=======

The context is where everything begins in Robotlegs. Instead of Extending a context (like in version on) you add your code to it. You do this with the plugin architecture using Extensions and Configs. From within these classes you also have (or can get reference to) the context.

The Context is made up of lots of classes. It exposes you with the following features.

- Injector
- Extension Installer
- Config Installer
- Pin
- Logging
- Lifecycle

Installing Extensions / Bundles
-------------------------------

With the context, all 3rd party content can be installed and also configured with robotlegs. The two interfaces you have to install are IBundle and IExtension.

However, you can install anything that has this method signature:

```csharp
public void Extend(IContext context)
{
}
```


The context follows the decorator pattern which returns itself for easier reading.

```csharp
new Context().Install<MVCSBundle>().Install<ExampleExtension>().Configure<ApplicationConfig>();
```

> Note: A lot of classes for setup in Robotlegs follow the decorator pattern

The install parameter takes a type, and will only ever install one type, the following example will only install the MVCSBundle once even though each install call method is valid.

```csharp
IContext context = new Context()
	.Install<MVCSBundle>()
    .Install(new MVCSBundle())
    .Install(typeof(MVCSBundle));	// MVCSBundle is installed only once
```

It's good to point out, a IBundle and IExtension have the same interface methods but should be treated very differently.

An Extension is for adding something to the Context whereas a Bundle is just a wrapper for easily installing many extensions and configurations.

For more detail about making your own extensions. Please check out the [Writing An Extension](../WritingAnExtension.md) section.

Configuring
-----------

The Config Manager is another class supplied by the robotlegs framework. This is similar to the Extension Installer, however the intention is to configure the mappings made in the previous extensions.

Here is how you would add anything to be configured to your context.

```csharp
IContext context = new Context()
	.Configure<Config1>()
    .Configure(new Config2())
    .Configure(typeof(Config3));
```

All of these objects passed I have passed into the Configure methods are of type **IConfig** which require the following method signature:

```csharp
public void Configure()
{
}
```

During the initiation phase of the Context all of the IConfigs are injected into with any dependencies and then have their 'Configure' method executed.

Here is an example of what an IConfig might look like:

```csharp
[Inject]
public IRateMeService _rateMeService;

[Inject]
public IAudioSoundLevels _audioSoundLevels;

public void Configure()
{
	_rateMeService.Setup(RateMeLaunchTimes.TWICE, RateMeDays.FIVE);
	_audioSoundLevels.FunctionNameHere();
}
```

Unlinke the Extension Installer. Configs of the same type can be configured multiple times, it will only prevent it from being configured if it has had the same instance.

```csharp
IContext context = new Context()
	.Configure(new MyConfig())
	.Configure(new MyConfig())
	.Configure(new MyConfig()); // Config will be configured three times
```

Lifecycle
---------

The context goes through a very specific lifecycle. A context starts of Uninitialzed, and can go through the following states:

- UNINITIALIZED
- INITIALIZING
- ACTIVE
- SUSPENDING
- SUSPENDED
- RESUMING
- DESTROYING
- DESTROYED

The context and only ever be initialized once, and destroyed once. After that, there is no reviving the context.

You can control the state with the following methods:

```csharp
IContext context = new Context();	// Context is UNINITIALIZED
context.Initialize();				// Context is ACTIVE
context.Suspend();					// Context is SUSPENDED
context.Resume();					// Context is ACTIVE
context.Destroy();					// Context is DESTROYED
```

If you have installed a platform specific bundle and not the defaut MVCSBundle. You will not have to Initialize or Destroy the context as the lifecycle will be synced with it's ContextView. See your [platform](../Platforms.md) documentation for more information.

Extension, Configs and the Lifecycle order
------------------------------------------

Now, the Extension Installer and the Config Manager can behave differently in the lifecycle and is important to konw.

Firstly, Extensions can only be installed before the context has been Initialized. If you attemp to install an extension after the context has been initialzed.

Even though you can install extensions after the context has been initialied. Some extensions rely on the 'BeforeInitializing' and 'WhenInitializing' callbacks which will error if added after initialization.

```csharp
new Context().Initialize().Install<MVCSBundle>(); // Throws error because of 'BeforeInitializing'
```

Because of this is advised you try to install your extensions before initialization.

The ConfigManager will process the IConfig files after initialization. Unlike the extensions which are processed immediatly.


```csharp
private IContext context;

public void SetupContext()
{
	context = new Context()
		.Install<Extension1>()
		.Install<Extension2>()
		.Configure<MyConfig1>()
		.Configure(new MyConfig2("extra data"));
	context.Initialize();
	context.Configure(typeof(MyConfig3));

	// Output:
    // Installed Extension 1
	// Installed Extension 2
	// Context Initialized
	// Configured Config 1
	// Configured Config 2
	// Configured Config 3
}
```

Install methods are run when executed. And if your configuration is an IConfig, it will be processed during the Initialize phase of the context.

> N.B. Anything processed by new Config Handlers (Not the default IConfig) will be processed immediatly and not wait for initialization.

Also, it's good to point out where all configs and states are handled in relation to the 'BeforeInitializing', 'WhenInitializing' and 'AfterInitializing' callbacks.

1. context.**Initialize()**;
2. Context State: **Initializing**
3. **BeforeInitializing** handlers processed
4. **WhenInitializing** handlers processed
5. Queued **IConfigs** are processed
6. **AfterInitializing** handlers processed
7. Context State: **Active**

Pin
---

The pin is a *tiny* class that will keep a resource in memory and prevent it from being garbage collected. It was primarilly added to Detain and Release a command, however you are free to use it for any other purpose.

The pin class has been added to the IContext interface:

```csharp
void Load()
{
	context.Detain(this);
}

void LoadComplete()
{
	context.Release(this);
}
```

Config Handlers
---------------
The config manager is the part of the context that manages all objects that are passed into the 'Configure' function call for the Context.

When items are passed into Configure. It is checked with all IMatchers on an object and if it passes, it can then then use the handler function on it.

Config manager adds two IMatcher and Handler functions to it.

```csharp
AddConfigHandler(new ClassMatcher(), HandleType);
AddConfigHandler(new ObjectMatcher(), HandleObject);
```

* Check to see if the object is a Type. If it is it, after initialization of the context it will reflect the type into IConfig and call configure().

* Check to see if the object is an IConfig. If it is it, after initialization of the context it will call configure().
You can add new IMatchers and Handlers for anything that is passed into configure.

Please be aware, that your IMatchers and IHandlers will not wait for the context to initialize before processing. This is a feature tied into the previous handlers.

Here is how you can add your own handlers for different types of IConfig:

```csharp
private void AddHandler()
{
	context.AddConfigHandler(new InstanceOfMatcher (typeof(XMLDocument)), HandleXMLInstaller);
}

public void HandleXML(obj xml)
{
	XMLDocument xmlDoc = xml as XMLDocument;
	XmlNode[] nodes = xmlDoc.SelectNodes("//configurations");
	foreach (XMLNode node node in nodes)
	{
		string assemblyQualifiedName = node.Value;
		Type type = Type.GetType(assemblyQualifiedName);
		context.Configure(type);
	}
}
```

AddChild & RemoveChild
----------------------

AddChild and RemoveChild allows it's Injector to be parented and unparented.

When an injector has a parent, if it cannot find a value directly it will ask for it from it's parent and so on until it finds a value or runs out of parents.

```csharp
Context parentContext = new Context();
Context childContext = new Context();
parentContext.AddChild(childContext);

parentContext.Injector.Map<Test>().AsSingleton();

Test test = childContext.GetInstance<Test>(); // Returns the value mapped on the parent context
```

If you add a child to a context. You can remove it as a parent injector by calling ```RemoveChild```. Or if you destory the child context the removal is handled for you.

For more information about how to communicate between contexts, see the [Modularity](./Modularity.md) section.


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
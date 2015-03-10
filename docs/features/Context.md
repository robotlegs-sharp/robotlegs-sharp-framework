
Context
==========

The context is the root of the framework with Robotlegs.
The context comes with a few things.

Extension Installer and Config Manager, which enable you to tie into the Context and setup and boot your own code and even install other people's code wrapped up nicely into extensions.

```csharp
private IContext context;

public void SetupContext()
{
	context = new Context()
	.Install<MVCSBundle>()
	.Install<Extension1>()
	.Install<Extension2>()
	.Configure<MyConfig1>()
	.Configure(new MyConfig2("extra data"));
	context.Initialize();
}
```

Regardless of the order, Install classes/instances are run first. If your configuration is an IConfig, it will be processed during the Initialize phase of the context.

In our example, you might see the following in your ouput:

```
Installed Extension 1
Installed Extension 2
Context Initialized
Configured Config 1
Configured Config 2
```

Bundles and Extensions
===================

With the context, all 3rd party content can be installed and also configured with robotlegs. The three interfaces you have access to are IBundle, IExtensions and IConfig. Each are their own place.

IBundle
---------

This is a wrapper for installing a set or group of extensions. It has the same functionaliy as IExtension but is named appropriatly. For example, we have MVCSBundle which installs all the extensions we use to handle our Model, View, Controller, Services code.

If you find yourself installing the same extensions and/or configurations together for your applications, perhaps you should consider grouping them into a bundle.

```csharp
namespace robotlegs.example.bundle
{
	public class MobileGameBundle
	{
		public void Extend(IContext context)
		{
			context.Install<RateMePopupExtension>();
			context.Install<AudioSoundLevelsExtension>();
			context.Install<ApplicationVersionUpgradeExtension>();
			context.Install<FacebookExtension>();
			context.Install<TwitterExtension>();

			context.Configure<RateMeAfterWeekConfig>();
		}
	}
}
```

IExtension
-------------

This gets passed the context so you are free to do any injecor mapping as soon as this boots up.

```csharp
public void Extend(IContext context)
{
	context.Injector.Map<IAudioSoundLevelModel>().ToSingleton<AudioSoundLevelModel>();
	context.Injector.Map<IAudioSoundLevelService>().ToSingleton<AudioSoundLevelService>();
}
```

If your extension is reliant on other extensions, you shoudln't try and get the other extension mappings from the the injector until all extensions have been Installed. This is why we don't inject into the extension for you as it will be possible to get a null object reference if you install your extensions in the wrong order.

Instead, you should be using the callbacks from the lifecycle during the initation process to communicate/use other extensions.

```csharp
private IInjector _injector;

public void Extend(IContext context)
{
	_injector = context.Injector;
	_injector.Map<MyExtension>().AsSingleton();

	context.BeforeInitializing(BeforeInitializing as Action);
}

public void BeforeInitializing()
{
	IDependable dependable = _injector.GetInstance<IDependable>();
	if (dependable != null)
	{
		dependable.SetMyExtension(_injector.GetInstance<MyExtension>());
	}
}
```

ConfigManager and IConfigs
=======================

The Config Manager is another class supplied by the robotlegs framework. This is similar to the Extension Installer, however here you get to configure the mapping made in the previous extensions.

IConfig
--------

The config manager installs a default IConfig handler for you. What this does is after the initiation phase when all the Extensions have been installed, is to inject into your IConfig any dependencies and then run their 'Configure' method.
Here is an example of what an IConfig might look like:

```csharp
[Inject]
public IRateMeService _rateMeService;
public IAudioSoundLevels _audioSoundLevels;

public void Configure()
{
	_rateMeService.Setup(RateMeLaunchTimes.TWICE, RateMeDays.FIVE);
	_audioSoundLevels.FunctionNameHere();
}
```

Configurations are run after the context has been Initialized.

Config Manager Handlers
------------------------------

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

```csharp
private void AddHandler()
{
	context.AddConfigHandler(new XMLMatcher, HandleXMLInstaller);
}

public void HandleXML(obj xml)
{
	XMLDocument xmlDoc = xml as XMLDocument;
	XmlNode[] nodes = xmlDoc.SelectNodes("//configurations");
	foreach (XMLNode node node in nodes)
	{
		string fullName = node.Value;
		Type type = SomeHowGetTypeFromFullName();
		context.Configure(type);
	}
}
```

And with the following IMatcher

```csharp
public class XMLMatcher : IMatcher
{
	public bool Matches(object obj)
	{
		if (object is XMLDocument)
			return true;
		return false;
	}
}
``` 

For more detail about making extensions. Please check out the [asdf]() section.


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
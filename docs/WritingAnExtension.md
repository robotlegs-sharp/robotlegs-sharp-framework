Writing An Extension
====================

Summary
-------

An extension is a snappable piece of funcationlity that can be 'Installed' to a Context. You can use this to easily resuse code within multiple projects.

File Structure
--------------

The extension files should be removable from your project easily. We like to keep the following strucutre:
```
ExampleExtensionFolder/readme.md
ExampleExtensionFolder/ExampleExtension.cs
ExampleExtensionFolder/ExampleConfig.cs
ExampleExtensionFolder/api/IExampleService.cs
ExampleExtensionFolder/api/ExampleServiceEvent.cs
ExampleExtensionFolder/impl/ExampleService.cs
ExampleExtensionFolder/impl/ExampleCommand.cs
```

Everything on the root folder, can be installed. Everything in the API your user (programmer) should have access to. Everything in impl, your user (programmer) should not need to use.

Where to start
--------------

An extension starts with an IExtension file. Here you can use the IInjector to map values of anything that is needed.

```csharp
public ExampleExtension : IExtension
{
	public void Install(IContext context)
    {
    	context.injector.Map<IMyExtensionService>().ToSingleton<MyExtensionService>();
    }
}
```

If you would like to grab a value from the injector (if it's dependable on other mapped values). You can use the ```context.Injector``` but as you don't know if the value has been mapped yet. Don't pull it from the Install function.

When can I get values from the injector?
----------------------------------------

It is easy to implement an IConfig to have injectable values (which you can use still). But it is better if you can use the callbacks as you have more control over when it will be executed.

- If you class needs to tell the context to **prevent from initializing**, or if you need to do **further setup** of items before the Configs/WhenHandlers then use the **BeforeInitializing**
- If you want to set something up **before you user config** is run then use the **WhenInitializing** handler.
- If you need to run **after your user's configuration** files, use the **AfterInitializing** handler.

For further clarification, the order in which these are run are:
- BeforeHandlers
- WhenHandlers
- Configs
- AfterHandlers

So as I only want to have my class avaliable for the user confiuration files, I'll invoke my setup in the WhenInitializing handler. Have a look at how our extension plugs into the MVCS below.

```csharp
public ExampleExtension : IExtension
{
	private IInjector _injector;

	public void Install(IContext context)
    {
		_injector = context.Injector;
    	_injector.Map<IMyExtensionService>().ToSingleton<MyExtensionService>();

		context.WhenInitializing(WhenInitializing as Action);
    }

	private void WhenInitializing()
	{
		IMediatorMap mediatorMap = _injector.GetInstance<IMediatorMap>();
		mediatorMap.Map<IExampleView>().ToMediator<IExampleMediator>();

		IEventCommandMap commandMap = _injector.GetInstance<IEventCommandMap>();
        commandMap.Map(ExampleEvent.Type.EXAMPLE).ToCommand<ExampleCommand>();
	}
}
```

Adding Configurations
---------------------

IConfig classes in your extension, should be more options that your user can configure. The configuration can either give your extension different/more functionality or optionally tie two extensions togther.

Here is a config that changes the functionality of ExampleExtension

```csharp
public class ExampleServiceUseMultiThreadConfig : IConfig 
{
	[Inject]
    public IExampleService service;

	public void Configure()
    {
    	service.UseMultiThread();
    }
}
```

And here is another config, that uses invokes a different command in another Extension alltogether.

```csharp
public class ExampleServiceTieToDifferentServiceConfig : IConfig
{
	[Inject]
    public IEventCommandMap commandMap;

	public void Configure()
    {
    	commandMap.Map(ExampleEvent.EXAMPLE_COMPLETE).ToCommand<ExecuteDifferentServiceCommand>();
    }
}
```

These configs, won't be installed by default. But your user can install them with the ```Configure``` method.

How to use
----------

When your extension is complete. Consider writing a readme.md file on how to use the extension properly.

All extensions should be installed like so:

```csharp
Context context = new Context()
	.Install<MVCSBundle>()
	.Install<ExampleService>()

context.Initialize();
```

And optional configurations can be added:

```csharp
Context context = new Context()
	.Install<MVCSBundle>()
	.Install<ExampleService>()
    .Configure<ExampleServiceTieToDifferentServiceConfig>()
    .Configure<ExampleServiceUseMultiThreadConfig>();

context.Initialize();
```

From here
---------

* [Readme](../README.md)
	* [A Brief Overview](./ABriefOverview.md)
	* [Features](./Features.md)
	* [Platforms](./Platforms.md)
	* [Writing An Extension](./WritingAnExtension.md)
	* [Common Problems](./CommonProblems.md)
	* [The internals (how it all works)](./TheInternals.md)
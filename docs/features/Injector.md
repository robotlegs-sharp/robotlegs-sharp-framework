
Injector
========

The injector is a core part of the Robotlegs framework. It is simply used to provide dependant objects at runtime.

It is useful when you want to give access something to an class, but have the ability to swap out that object at a later date in future projects.


Setting up an injection mapping
-------------------------------

In order to have things that are injectable, you need to set up some rules for the injector to follow in order to provide you with values. There are a few ways to set up your injectable values.

#### When I ask for a Car, always give me the same one

```csharp
injector.Map<Car>().AsSingleton();
```

####  When I ask for a Car, give me a new one every time

```csharp
injector.Map<Car>().ToType<Car>();
```

#### When I ask for a Car, give me _this_ one

```csharp
Car thisCar = new Fiat(Color.GREEN, Engine.CC_476, Transmission.MANUAL);
injector.Map<Car>().ToValue(thisCar);
```

#### When I ask for a Car, give me the same Aston Martin every time

```csharp
injector.Map<Car>().ToSingleton<AstonMartin>();
```

#### When I ask for _my_ Car, give me my one

```csharp
Car myCar = new Car();
injector.Map<Car>(Key.MINE).ToValue(myCar);
```


Getting values with [Inject]
----------------------------

The injector will scan your class for properties and fields with the ```[Inject]``` tag.

By default, anything with ```[Inject]``` must be mapped with a rule before being satisfied. If not,the injector will not know what to provide and it will throw an error.
If the optional attribute is applied ```[Inject(true)]``` , the Injector will not error if it cannot find a mapping and will leave the value as null.

The property also has to be **public** or else the injector won't provide you the value.

These injectable values will not be available to you the constructor. If you need to know when they are avalible, create a ```[PostConstruct]``` tag on a method. This method will be called by the injector after values have has been provided. 

If you can, use the framework after injection methods [here](#Framework classes that are injected into)

An example class with the inject tag, named inject tag, optional inject tag and PostConstructMethod.

```csharp
public class TestClass
{
	[Inject]
	public IValue injectedValue;
	
	[Inject(Key.MINE)]
	public IValue namedValue;
	
	[Inject(true)]
	public INotRequiredValue optionalValue;

	public IValue nonInjectedValue;

	public TestClass()
	{
		// Values not injected yet...
	}

	[PostConstruct]
	public void AfterInjection()
	{
		Console.WriteLine("Value: " + injectedValue);
		Console.WriteLine("Named Value: " + namedValue);
		Console.WriteLine("Optional Value: " + optionalValue);
	}
}
```

If you still would like the values to be given to you in the constructor. You can pass in interfaces into the constructor. By default SwiftSuspenders injector will try to inject the largest constructor. If you have multiple constructors. Adding an ```Inject``` tag will make the injector use that constructor.

```csharp
public class TestClass
{
	public TestClass(IValue injectedValue)
	{
		Console.WriteLine("Value: " + injectedValue);
	}
}
```

Of course, constructor injection will not work if you mapped to value:
```injector.Map<Class>().ToValue(value);``` 
As the injector would not have created the instance.


Get values from Injector
------------------------

If you have the injector. You can also get your values from it.
You most likely will use this when creating an extension as you will given an injector via the context.

```csharp
Car car = injector.GetInstance<Car>();
```

And to get a value with a key:

```csharp
Car myCar = injector.GetInstance<Car>(Keys.MINE); 
```


Framework classes that are injected into
----------------------------------------

Throughout the application the following classes are injected into.

* IConfigs - _Injected just before Configure method_
* Mediators - _Injected just before the Initialize method_
* Commands - _Injected just before the Execute method_
* Models and Services - _Injected into only if mapping rule has been set with the injector._
	_To have access to injected properties at the earliest avaliable point, please use the [PostConstruct] tag_
* Hooks - _Injected just before the Hook method_
* Guards - _Injected just before the Approve method_
* ViewProcess - Injected into just before the _Process_ method.

If you want to inject anything outside of this framework. You can use the following:

```csharp
TestClass testClass = new TestClass();
injector.InjectInto(testClass);
```

You could also get the injector to instantiate the class for you which would allow for constructor injection:

```csharp
TestClass testClass = injector.InstantiateUnmapped<TestClass>();
```

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
	* [Platforms](../Platforms.md)
	* [Writing An Extension](../WritingAnExtension.md)
	* [Common Problems](../CommonProblems.md)
	* [The internals (how it all works)](../TheInternals.md)
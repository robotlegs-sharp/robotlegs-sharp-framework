
Common Problems
==============

Mediator not initialised?
-------------------------

1. Is the view calling ViewNotifier.RegisterView()?
2. Is the mediator mapped?
3. Is the view in the correct place and picking the correct context view?

Injection Exception
-------------------

Injection exeptions can be *fun* when you first read them. By default, an ```[Inject]``` tag will thrown an error if it cannot find a dependency, so you'll get these a lot to begin with. The error gives you a lot of information about where it found the execption, as well as what it couldn't populate.

Some people get stuck thinking the target is the issue. Not it's injectable parameter.

Let's take the following lovely error:

```
InjectorMissingMappingException: Injector is missing a mapping to handle injection into property 'test' of object 'TestConfig' with type 'TestConfig'. Target dependency: '[MappingId: type=UnityEngine.GameObject, key=]'
```

Now this is saying, even though the Error states 'TestConfig' all over. It's saying that it cannot handle the Injection parameter for property **test** of type **GameObject** from in the class TestConfig. 

So this is the cause of the exception:

```csharp
[Inject]
public GameObject test;
```

And to fix this, I would need to map a some a in the injector to handle the class 'GameObject':

```
GameObject gameObjectSingleton = new GameObject();
injector.Map<GameObject>().ToValue(gameObjectSingleton);
```

Injection Null
--------------

If at any point, you are accessing a parameter that has been injected, and it's value is null. Most likely the Injector hasn't run on this class yet.

A lot of people have this issues when putting their command contents in a constructor and not the 'Execute' method.

```
public class BadCommand
{
	[Inject]
    public IInjector injectedParameter;

	public BadCommand()
    {
    	Console.WriteLine("Why is injected parameter " + injectedParameter.ToString() + "?");
		// Output: Why is injected paramater null?
    }
}
```

If you want to know when the injector runs on Robotlegs classes, [look here](./features/Injector.md#Framework-classes-that-are-injected-into).

From here
---------

* [Readme](../README.md)
	* [A Brief Overview](./ABriefOverview.md)
	* [Features](./Features.md)
	* [Platforms](./Platforms.md)
	* [Writing An Extension](./WritingAnExtension.md)
	* [Common Problems](./CommonProblems.md)
	* [The internals (how it all works)](./TheInternals.md)
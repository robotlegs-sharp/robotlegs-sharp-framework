
View Processor
=========

The View Processor Map allows you to do any sort of action on a view when it is registerd or unregistered. This enables the user to do _anything_ with the filtering of IMatchers to certain views.

Like most of mappers, you should generally map processors in the setup phase of robotlegs.

Basic Mapping
-------------

You can setup a simple procesor for all 'ScaleableView' instances like so:

```csharp
viewProcessorMap.Map<ScaleableView>().ToProcess<DoubleSizeProcessor>();
```

With this mapping setup, any ScaleableView that gets registered will have the process 'DoubleSizeProcessor' run.

A View Process
--------------

Here is an example of a Process that scales the view to double it's size. A process isn't tied to an interface or base class, it just calls the methods ```Process``` and ```Unprocess``` if avaliable.

Injection is also applied to all Process's classes The 'view' is also injectable by the types you have mapped it to (in my case, the 'ScaleableView' class). Note, the class is only injected once before 'Process' and not before 'Unprocess'.

```csharp
public class DoubleSizeProcessor
{
	private float originalScale;

	[Inject]
    public ScaleableView scaleableView;

    public void Process(object view, Type type, object injector)
    {
    	Console.WriteLine("Sweet, just got a " + view);

		originalScale = scaleableView.Scale;
		scaleableView.SetScale(originalScale * 2);
    }

    public void Unprocess(object view, Type type, object injector)
    {
    	Console.WriteLine("Bye bye " + view + ", I'll miss your reference");

		scaleableView.SetScale(originalScale);
    }
}
```

> Note: I could have easily casted and used the view object (view as ScaleableView), but injected to cast the value instead.

Advanced Mapping
----------------

You have access to the IMatcher system for mapping.

```csharp
viewProcessorMap.Map(new TypeMatcher().AllOf(typeof(A), typeof(B)).NoneOf(typeof(C)).AnyOf(typeof(D), typeof(E))).ToProcess<ExampleProcess>();
```

You can read more about the [IMatching System here](./Mediators.md#Mapping With IMatcher).

### Guards

Guards can also be added to the mapping configuration, to prevent processes from running

```csharp
viewProcessorMap.Map<ScaleableView>().ToProcess<DoubleSizeProcessor>().WithGuards<WhenInGameGuard>();
```

For more information on Guards see the [Guards](./Guards.md) section.

### Hooks

You can also do actions on your processors/views just before the 'Process' function. You can add a hook like so:

```csharp
viewProcessorMap.Map<ScaleableView>().ToProcess<DoubleSizeProcessor>().WithGuards<WhenInGameGuard>().WithHooks<CallThisBeforeMapping>();
```

For more information on Hooks see the [Hooks](./Hooks.md) section.

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
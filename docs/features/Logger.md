
Logger
======

The logger is an inbuilt tool for logging out from your code.
It inbuilt features for five different severitys of logging.

* DEBUG
* INFO
* WARN
* ERROR
* FATAL


It is also possible to output the logs into multiple different API's.

The logger also takes a source target, so you are always aware of which part of the app the log origniated from.

If you ever write and extension it is highly reccomended that you use the logger so that the user of the extension has more information of what is going on.


Getting a logger
----------------

If your class is [is injected into](./Injector.md#Framework classes that are injected into) then you can easily get an injector with the [Inject] tag.

```csharp
[Inject]
public ILogger logger;
```

You can pull it from the context, but need to tell it what class you are logging from.

```csharp
ILogger logger = context.GetLogger(this);
```

How to Log
----------

The logger has 5 methods to log to.

```csharp
logger.Debug("Just debugged inside {0} class", new object[]{this}));
logger.Info("Just a friendly message, telling you what's going on {0} and {1}", new object[]{this, instance});
logger.Warn("Whoops, shouldn't be doing this really");
logger.Error("Ahhhhhhhh! 0_o");
logger.Fatal("R.I.P");
```

The user won't see any of this depending on the LogLevel. Setting the LogLevel will prevent logs being sent to all of it's targets.

You can change the loglevel in the context.

```csharp
context.LogLevel = LogLevel.DEBUG;	// Will show all logs
context.LogLevel = LogLevel.INFO;	// Will show Fatal, Error, Warn and Info logs.
context.LogLevel = LogLevel.WARN;	// Will show Fatal, Error and Warn logs.
context.LogLevel = LogLevel.ERROR;	// Will show Fatal and Error logs.
context.LogLevel = LogLevel.FATAL;	// Will show Fatal logs.
```

> INFO: In the standard MVCSBundle, the LogLevel is set to INFO and will throw and the Viligence Extension will throw an exeption for Warn, Error and Fatal.

Output Logs
-----------

So, you want to use the logs too? Cool. All you need is an ILogTarget.

The context has a LogManager, which manages all ILogTargets. The syntax is simple:

```csharp
public class ChatroomLogger : ILogTarget
{
	public Log(object source, LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
	{
		string name = source.GetType().Name;
        string talk = "";
        switch (level)
        {
			case LogLevel.DEBUG:
				talk = "ranted";
                break;
			case LogLevel.INFO:
            	talk = "sez";
				break;
			case LogLevel.WARN:
            	talk = "threatened";
				break;
			case LogLevel.ERROR:
            	talk = "was stunned and said";
				break;
			case LogLevel.FATAL:
            	talk = "just died uttering the words";
				break;
        }
        Console.WriteLine(name + " " + talk + " " + message, messageParameters);
	}
}
```

The best I personally like about the logger is the potential to connect to other API's or a visual log for those hard to debug issues.

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
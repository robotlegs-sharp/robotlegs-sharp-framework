# Robotlegs Sharp Framework

This is the home of the C# port of [Robotlegs] _(Version 2)_.

I've tried my best to keep the code as close to the original as possible whilst keeping an open mind about modifying the structure only to take benefits of the new language or to make it more flexible due to missing features available from it's previous language.

## Creating A Context

To create a Robotlegs application or module you need to instantiate a Context. A context won't do much without some configuration.

Plain ActionScript:

```csharp
_context = new Context()
    .install(MVCSBundle)
    .configure(MyAppConfig, SomeOtherConfig)
    .configure(new ContextView(this));
```

We install the MVCSBundle, which in turn installs a number of commonly used Extensions. We then add some custom application configurations.

We pass the instance of the root display object through as the "contextView" which is required by many of the view related extensions. It must be installed after the bundle or it won't be processed. Also, it should always be added as the final configuration as it may trigger context initialization.

### Todo: Talk about IParentFinder, and FallbackContainer

Note: You must hold on to the context instance or it will be garbage collected.

[Framework](https://github.com/robotlegs/robotlegs-framework/tree/master/src/robotlegs/bender/framework)

## Context Initialization

If a ContextView is provided the Context is automatically initialized when the supplied view lands on stage. Be sure to install the ContextView last, as it may trigger context initialization.

If a ContextView is not supplied then the Context must be manually initialized.

```csharp
_context = new Context()
    .Install<MyCompanyBundle>()
    .Configure<MyAppConfig>()
	.Configure<SomeOtherConfig>()
    .Initialize();
```

Note: This does not apply to Flex MXML configuration as the ContextView is automatically determined and initialization will be automatic.

[ContextView](https://github.com/robotlegs/robotlegs-framework/tree/master/src/robotlegs/bender/extensions/contextView)

## Application & Module Configuration

A simple application configuration file might look something like this:

```csharp
public class MyAppConfig implements IConfig
{
    [Inject]
    public IInjector injector;

    [Inject]
    public IMediatorMap mediatorMap;

    [Inject]
    public IEventCommandMap commandMap;

    [Inject]
    public ContextView contextView;

    public function configure():void
    {
        // Map UserModel as a context enforced singleton
        injector.Map<UserModel>().AsSingleton();

        // Create a UserProfileMediator for each UserProfileView
        // that lands inside of the Context View
        mediatorMap.Map<UserProfileView>().ToMediator<UserProfileMediator>();

        // Execute UserSignInCommand when UserEvent.SIGN_IN
        // is dispatched on the context's Event Dispatcher
        commandMap.Map(UserEvent.Type.SIGN_IN).ToCommand<UserSignInCommand>();

        // The "view" property is a object reference so would 
		// need to cast it to add a view
        (contextView.view as Transform).AddComponent<MainView>();
    }
}
```

The configuration file above implements IConfig. An instance of this class will be created automatically when the context initializes.

We Inject the utilities that we want to configure, and add our Main View to the Context View.

[Framework](https://github.com/robotlegs/robotlegs-framework/tree/master/src/robotlegs/bender/framework)

### An Example Mediator

The mediator we mapped above might look like this:

```as3
public class UserProfileMediator : Mediator
{
    [Inject]
    public UserProfileView view;

    override public void Initialize()
    {
        // Redispatch an event from the view to the framework
        AddViewListener(UserEvent.Type.SIGN_IN, dispatch);
    }
}
```

The view that caused this mediator to be created is available for Injection.

[MediatorMap](https://github.com/robotlegs/robotlegs-framework/tree/master/src/robotlegs/bender/extensions/mediatorMap)

### An Example Command

The command we mapped above might look like this:

```as3
public class UserSignInCommand : ICommand
{
    [Inject]
    public UserEvent event;

    [Inject]
    public UserModel model;

    public void execute()
    {
        if (event.Username == "bob")
            model.SignedIn = true;
    }
}
```

The event that triggered this command is available for Injection.

[EventCommandMap](https://github.com/robotlegs/robotlegs-framework/tree/master/src/robotlegs/bender/extensions/eventCommandMap)

# Building and Running the Tests

## Requirements

We have been building and running the Unit Tests on [Xamarin Studio] (http://xamarin.com/studio)
We are using the [NUnit] (http://www.nunit.org/index.php?p=download) and [Moq] (https://github.com/Moq/moq4) frameworks for our Unit Tests.

With Xamarin Studio installed, open up 'robotlegs-sharp-framework-test.sln'.

Open up the Unit Test Panel: View > Pads > Unit Tests

Within the unit test panel, click 'Run All' to run all the unit tests

## Other C# Robotlegs Alternatives

If you are interested in the Robotlegs framework, please consider looking at the other candiates.

- MonoArms (A C# port of Robotlegs 1.0)
- Strange IoC (A robotlegs inspired framework for Unity3D)
- Tiny IoC (A very small but exceptionally handy injector)

# Final notes

Thanks to all the hard work to the [original team] (https://github.com/robotlegs/robotlegs-framework/graphs/contributors) who wrote [Robotlegs 2] (https://github.com/robotlegs/robotlegs-framework/) for Actionscript for your clean optimised but complex code.
I both love and hate you guys <3
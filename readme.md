# Robotlegs Sharp Framework

This is the home of the C# port of [Robotlegs] _(Version 2.0)_.

I've tried my best to keep the code as close to the orignal as possible whilst keeping an open mind about modifying the structure only to take benefits of the new language or to make it more flexible due to missing features avaliable from it's previous language.

One of the reasons I've tried to keep the framework as close to the original is due to it's new bundle/extension package manager. With this it is possible for anyone else to modify/change the current structure as they see fit to add new features to the framework.

## Other C# Robotlegs Alternatives
If you are interested in the Robotlegs framework, please consider looking at the other candiates.

- MonoArms (A C# port of Robotlegs 1.0)
- Strange IoC (A robotlegs inspired framework for Unity3D)
- Tiny IoC (A very small but exceptionally handy injector)

### Transition period

Currently, this code base started as the [Strange-IOC] framework which is a robotlegs inspired framwork. I originally started to just write the extension manager to help manage the new features approaching the framework. But, as I started writing this framwork I started to find that although the package manager was successful, it didn't intergrate well with the old format or the Robotlegs 1/Strange IOC context. More things became dependant and I ended up changing a lot more than anticipated. Instead of forcing these changes onto Strange IOC, I took the opportunity to try and port all the nice features from 2.0 and try to stay as close to the original framework (within reason).

So currently I have a lot of code ported from Robotlegs 2.0 and a few very core extensions from Strange IOC working in the mix. These extensions all together works (and I am currently using in my projects) however I am re-writing the last few core Robotlegs extensions to replace the Strange IOC extensions.

### Roadmap

The last few things to do on the robotlegs port are:

- Injection Binder to Injection Map
- Port the command map
- Make the event command map
- Event map in mediators (for weak event listeners)
- Multi-Context Support


# Quickstart

So, you want to start a Robotlegs project?

## Install and Configure

For more information about the extension manager, please see this article.

## Application Configuration
Here is where you should configure you're application.

## Injection Rules (Models, Services)

Setup your models, and services as singletons here

## View Mediation

Here you can tie in you're views to your mediators

## Events to Commands

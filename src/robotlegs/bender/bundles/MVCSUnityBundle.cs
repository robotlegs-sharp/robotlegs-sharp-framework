using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.contextview;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.mediatorMap;
using robotlegs.bender.extensions.enhancedLogging;
using robotlegs.bender.extensions.debugLogging;
using robotlegs.bender.framework.impl;

namespace robotlegs.bender.bundles
{
	public class MVCSUnityBundle : IExtension
	{
		public void Extend(IContext context)
		{
			context.LogLevel = LogLevel.INFO;
			
			context.Install<ConsoleLoggingExtension>();
			context.Install<DebugLoggingExtension>();

//			context.injector.Map (typeof(IContext)).ToValue (context);//.ToName(ContextKeys.CONTEXT);

			context.Install<ContextViewExtension> ();
			//injectionBinder.Bind<GameObject>().ToValue(contextView).ToName(ContextKeys.CONTEXT_VIEW);

			context.Install<ViewManagerExtension>();
			context.Install<MediatorMapExtension>();

			context.Configure<ContextViewListenerConfig>();
		}
	}
}


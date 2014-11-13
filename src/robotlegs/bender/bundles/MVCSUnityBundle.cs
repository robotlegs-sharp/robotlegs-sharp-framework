using System;
using robotlegs.bender.framework.api;
using stange.extensions.contextview;
using robotlegs.bender.extensions.contextview;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.mediatorMap;
using robotlegs.bender.extensions.enhancedLogging;
using robotlegs.bender.extensions.debugLogging;
using robotlegs.bender.framework.impl;
using strange.framework.context.api;

namespace robotlegs.bender.bundles
{
	public class MVCSUnityBundle : IExtension
	{
		public void Extend(IContext context)
		{
			context.LogLevel = LogLevel.INFO;
			
			context.Install<ConsoleLoggingExtension>();
			context.Install<DebugLoggingExtension>();

			context.injectionBinder.Bind<IContext>().ToValue(context).ToName(ContextKeys.CONTEXT);

			context.Install<ContextViewExtension> ();
			//injectionBinder.Bind<GameObject>().ToValue(contextView).ToName(ContextKeys.CONTEXT_VIEW);

			context.Install<ViewManagerExtension>();
			context.Install<MediatorMapExtension>();

			context.Configure<ContextViewListenerConfig>();
		}
	}
}


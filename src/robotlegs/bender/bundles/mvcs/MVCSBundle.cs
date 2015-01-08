using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.enhancedLogging;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.mediatorMap;
using robotlegs.bender.extensions.contextview;
using robotlegs.bender.extensions.vigilance;
using robotlegs.bender.extensions.directCommandMap;
using robotlegs.bender.extensions.eventCommandMap;
using robotlegs.bender.extensions.eventDispatcher;
using robotlegs.bender.extensions.localEventMap;

namespace robotlegs.bender.bundles.mvcs
{
	public class MVCSBundle : IExtension
	{
		public void Extend(IContext context)
		{
			/**
			 * Robotlegs
			 */
			context.LogLevel = LogLevel.INFO;

			context.Install(typeof(ConsoleLoggingExtension));
			context.Install(typeof(VigilanceExtension));
//			context.Install(typeof(InjectableLoggerExtension));
			context.Install(typeof(ContextViewExtension));
			context.Install(typeof(EventDispatcherExtension));
//			context.Install(typeof(ModularityExtension));
			context.Install(typeof(DirectCommandMapExtension));
			context.Install(typeof(EventCommandMapExtension));
			context.Install(typeof(LocalEventMapExtension));
			context.Install(typeof(ViewManagerExtension));
//			context.Install(typeof(StageObserverExtension));
			context.Install(typeof(MediatorMapExtension));
//			context.Install(typeof(ViewProcessorMapExtension));
//			context.Install(typeof(StageCrawlerExtension));
//			context.Install(typeof(StageSyncExtension));

			context.Configure(typeof(ContextViewListenerConfig));
		}
	}
}


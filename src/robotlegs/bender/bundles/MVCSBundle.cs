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

namespace robotlegs.bender.bundles
{
	public class MVCSBundle : IExtension
	{
		public void Extend(IContext context)
		{
			/**
			 * Robotlegs
			 */
			context.LogLevel = LogLevel.INFO;

			context.Install<ConsoleLoggingExtension>();
			context.Install<VigilanceExtension>();
//			context.Install<InjectableLoggerExtension>();
			context.Install<ContextViewExtension>();
			context.Install<EventDispatcherExtension>();
//			context.Install<ModularityExtension>();
			context.Install<DirectCommandMapExtension>();
			context.Install<EventCommandMapExtension>();
//			context.Install<LocalEventMapExtension>();
			context.Install<ViewManagerExtension>();
//			context.Install<StageObserverExtension>();
			context.Install<MediatorMapExtension>();
//			context.Install<ViewProcessorMapExtension>();
//			context.Install<StageCrawlerExtension>();
//			context.Install<StageSyncExtension>();

			context.Configure<ContextViewListenerConfig>();
		}
	}
}


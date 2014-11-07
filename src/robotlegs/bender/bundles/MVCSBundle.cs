using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.enhancedLogging;
using strange.extensions.command;
using strange.framework.context.api;
using strange.extensions.dispatcher;
using stange.extensions.dispatcher;
using strange.extensions.sequencer;
using stange.extensions.contextview;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.mediatorMap;
using robotlegs.bender.extensions.contextview;
using robotlegs.bender.extensions.vigilance;
using robotlegs.bender.extensions.directCommandMap;

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
//			context.Install<EventDispatcherExtension>();
//			context.Install<ModularityExtension>();
			context.Install<DirectCommandMapExtension>();
//			context.Install<EventCommandMapExtension>();
//			context.Install<LocalEventMapExtension>();
			context.Install<ViewManagerExtension>();
//			context.Install<StageObserverExtension>();
			context.Install<MediatorMapExtension>();
//			context.Install<ViewProcessorMapExtension>();
//			context.Install<StageCrawlerExtension>();
//			context.Install<StageSyncExtension>();

			context.Configure<ContextViewListenerConfig>();

			/*
			 * Strange IOC
			 */
			context.injectionBinder.Bind<IContext>().ToValue(context).ToName(ContextKeys.CONTEXT);
			context.Install<EventCommandBinderExtension>();
			context.Install<DispatcherExtension> ();
			context.Install<SequencerExtension>();
			context.Configure<ContextDispatcherConfig> ();
			context.Configure<CommandBinderDispatchConfig>();
			context.Configure<SequencerDispatchConfig>();
		}
	}
}


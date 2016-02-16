//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
﻿
using robotlegs.bender.extensions.contextview;
using robotlegs.bender.extensions.directCommandMap;
using robotlegs.bender.extensions.enhancedLogging;
using robotlegs.bender.extensions.eventCommandMap;
using robotlegs.bender.extensions.eventDispatcher;
using robotlegs.bender.extensions.localEventMap;
using robotlegs.bender.extensions.mediatorMap;
using robotlegs.bender.extensions.modularity;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewProcessorMap;
using robotlegs.bender.extensions.vigilance;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using robotlegs.bender.platforms.unity.extensions.contextview;
using robotlegs.bender.platforms.unity.extensions.debugLogging;
using robotlegs.bender.platforms.unity.extensions.unityMediatorManager;
using robotlegs.bender.platforms.unity.extensions.unitySingletons;
using robotlegs.bender.platforms.unity.extensions.viewManager;
using robotlegs.bender.platforms.unity.extensions.viewManager.impl;
using UnityEngine;

namespace robotlegs.bender.bundles
{
	/// <summary>
	/// Unity single context bundle, installs the MVCS packages for a single context
	/// </summary>

	public class UnitySingleContextBundle : IBundle
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.LogLevel = LogLevel.INFO;

			if (Application.isEditor)
			{
				context.Install (typeof(UnitySingletonsExtension));
				context.Install (typeof(UnityMediatorManagerExtension));
			}

			context.Install(typeof(DebugLoggingExtension));
			context.Install(typeof(VigilanceExtension));
			context.Install(typeof(InjectableLoggerExtension));
			context.Install(typeof(UnityParentFinderExtension));
			context.Install(typeof(ContextViewExtension));
			context.Install(typeof(ContextViewTransformExtension));
			context.Install(typeof(EventDispatcherExtension));
			context.Install(typeof(UnityViewStateWatcherExtension));
			context.Install(typeof(DirectCommandMapExtension));
			context.Install(typeof(EventCommandMapExtension));
			context.Install(typeof(LocalEventMapExtension));
			context.Install(typeof(ViewManagerExtension));
			context.Install(typeof(MediatorMapExtension));
			context.Install(typeof(ViewProcessorMapExtension));
			context.Install(typeof(StageCrawlerExtension));
			context.Install(typeof(StageSyncExtension));
			context.Configure(typeof(ContextViewListenerConfig));

			// Fallback providers
			context.Configure(typeof(UnityFallbackStageCrawlerConfig));
			context.Configure(typeof(FallbackContainerConfig));
		}
	}
}
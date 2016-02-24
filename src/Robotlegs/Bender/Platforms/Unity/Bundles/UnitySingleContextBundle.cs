//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
﻿
using Robotlegs.Bender.Extensions.ContextViews;
using Robotlegs.Bender.Extensions.DirectCommand;
using Robotlegs.Bender.Extensions.EnhancedLogging;
using Robotlegs.Bender.Extensions.EventCommand;
using Robotlegs.Bender.Extensions.EventManagement;
using Robotlegs.Bender.Extensions.LocalEventMap;
using Robotlegs.Bender.Extensions.Mediation;
using Robotlegs.Bender.Extensions.Modularity;
using Robotlegs.Bender.Extensions.ViewManagement;
using Robotlegs.Bender.Extensions.ViewProcessor;
using Robotlegs.Bender.Extensions.Vigilance;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Platforms.Unity.Extensions.ContextViews;
using Robotlegs.Bender.Platforms.Unity.Extensions.DebugLogging;
using Robotlegs.Bender.Platforms.Unity.Extensions.Mediation;
using Robotlegs.Bender.Platforms.Unity.Extensions.UnitySingletons;
using Robotlegs.Bender.Platforms.Unity.Extensions.ViewManager;
using Robotlegs.Bender.Platforms.Unity.Extensions.ViewManager.Impl;
using UnityEngine;

namespace Robotlegs.Bender.Platforms.Unity.Bundles
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
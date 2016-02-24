//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

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

namespace Robotlegs.Bender.Bundles.MVCS
{
	public class MVCSBundle : IExtension
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend(IContext context)
		{
			/**
			 * Robotlegs
			 */
			context.LogLevel = LogLevel.INFO;

			context.Install(typeof(ConsoleLoggingExtension));
			context.Install(typeof(VigilanceExtension));
			context.Install(typeof(InjectableLoggerExtension));
			context.Install(typeof(EventDispatcherExtension));
			context.Install(typeof(ModularityExtension));
			context.Install(typeof(DirectCommandMapExtension));
			context.Install(typeof(EventCommandMapExtension));
			context.Install(typeof(LocalEventMapExtension));
			context.Install(typeof(ViewManagerExtension));
			context.Install(typeof(MediatorMapExtension));
			context.Install(typeof(ViewProcessorMapExtension));

			context.Configure(typeof(FallbackContainerConfig));
		}
	}
}
//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Extensions.ViewManagement.Impl;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.ViewManagement
{
	public class FallbackContainerConfig : IConfig
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public ContainerRegistry registry;

		[Inject]
		public IViewManager viewManager;

		[Inject]
		public ILogging logger;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Configure ()
		{
			if (registry.FallbackBinding != null) 
			{
				logger.Warn ("The fallback container has already been set in the registry");
			}

			viewManager.SetFallbackContainer (new object ());
		}
	}
}


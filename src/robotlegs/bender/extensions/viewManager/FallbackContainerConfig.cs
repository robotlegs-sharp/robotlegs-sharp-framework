//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.viewManager.impl;

namespace robotlegs.bender.extensions.viewManager
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
		public ILogger logger;

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


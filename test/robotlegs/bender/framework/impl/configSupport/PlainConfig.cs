//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace robotlegs.bender.framework.impl.configSupport
{
	public class PlainConfig
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("callback")]
		public Action<PlainConfig> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		[PostConstruct]
		public void init()
		{
			callback(this);
		}
	}
}


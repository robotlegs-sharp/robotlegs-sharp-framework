//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl.contextSupport
{
	public class CallbackConfig : IConfig
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action _callback;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public CallbackConfig(Action callback)
		{
			_callback = callback;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Configure()
		{
			_callback();
		}
	}
}


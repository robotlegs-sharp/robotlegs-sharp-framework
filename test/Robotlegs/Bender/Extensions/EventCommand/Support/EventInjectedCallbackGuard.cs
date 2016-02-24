//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using Robotlegs.Bender.Extensions.EventManagement.API;

namespace Robotlegs.Bender.Extensions.EventCommand.Support
{
	public class EventInjectedCallbackGuard
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IEvent evt;

		[Inject("ApproveCallback")]
		public Action<EventInjectedCallbackGuard> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public bool Approve()
		{
			callback(this);
			return true;
		}
	}
}


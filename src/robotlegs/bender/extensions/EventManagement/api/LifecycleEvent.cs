//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.EventManagement.Impl;

namespace Robotlegs.Bender.Extensions.EventManagement.API
{
	public class LifecycleEvent : Event
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public enum Type
		{
			ERROR,
			STATE_CHANGE,
			PRE_INITIALIZE,
			INITIALIZE,
			POST_INITIALIZE,
			PRE_SUSPEND,
			SUSPEND,
			POST_SUSPEND,
			PRE_RESUME,
			RESUME,
			POST_RESUME,
			PRE_DESTROY,
			DESTROY,
			POST_DESTROY
		}

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public LifecycleEvent (Type type) : base(type)
		{

		}
	}
}


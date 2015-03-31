//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class SelfReportingCallbackHook
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public SelfReportingCallbackCommand command;

		[Inject("HookCallback")]
		public Action<SelfReportingCallbackHook> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Hook()
		{
			callback(this);
		}
	}
}


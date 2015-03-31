//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class OptionalInjectionPointsCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		[Inject(true)]
		public string message;

		[Inject(true)]
		public int code;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			reportingFunc(message);
			reportingFunc(code);
		}
	}
}


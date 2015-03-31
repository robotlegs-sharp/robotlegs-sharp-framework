//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.commandCenter.api;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class AbstractInterfaceImplementingCommand : ICommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			reportingFunc(typeof(AbstractInterfaceImplementingCommand));
		}
	}
}


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.DirectCommand.API;

namespace Robotlegs.Bender.Extensions.DirectCommand.Support
{
	public class DirectCommandMapReportingCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IDirectCommandMap dcm;

		[Inject("ReportingFunction")]
		public Action<IDirectCommandMap> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			reportingFunc(dcm);
		}
	}
}


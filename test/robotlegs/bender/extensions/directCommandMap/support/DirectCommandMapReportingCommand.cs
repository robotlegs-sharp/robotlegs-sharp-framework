//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.directCommandMap.api;

namespace robotlegs.bender.extensions.directCommandMap.support
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


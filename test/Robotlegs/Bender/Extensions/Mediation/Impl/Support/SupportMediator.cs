//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.ViewManagement.Support;

namespace Robotlegs.Bender.Extensions.Mediation.Impl.Support
{
	public class SupportMediator
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public MediatorWatcher mediatorWatcher {get;set;}

		[Inject]
		public SupportView view {get;set;}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Initialize()
		{
			mediatorWatcher.Notify("SupportMediator");
		}

		public void Destroy()
		{
			mediatorWatcher.Notify("SupportMediator destroy");
		}
	}
}


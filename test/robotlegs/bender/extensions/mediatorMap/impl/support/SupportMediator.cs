//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.viewManager.support;

namespace robotlegs.bender.extensions.mediatorMap.impl.support
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


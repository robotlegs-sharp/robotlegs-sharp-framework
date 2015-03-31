//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportEventView : SupportView
	{
		public IEventDispatcher dispatcher = new EventDispatcher();

		public SupportEventView ()
		{
		}
	}
}


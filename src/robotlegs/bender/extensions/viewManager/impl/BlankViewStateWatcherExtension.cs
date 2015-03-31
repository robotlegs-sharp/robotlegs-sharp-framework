//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender
{
	public class BlankViewStateWatcherExtension : ViewStateWatcherExtension
	{
		protected override IViewStateWatcher GetViewStateWatcher (object contextView)
		{
			return null;
		}
	}
}


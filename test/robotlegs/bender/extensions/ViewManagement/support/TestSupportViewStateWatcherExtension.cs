//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using Robotlegs.Bender.Extensions.ViewManagement.API;

namespace Robotlegs.Bender.Extensions.ViewManagement.Support
{
	public class TestSupportViewStateWatcherExtension : ViewStateWatcherExtension
	{
		protected override IViewStateWatcher GetViewStateWatcher(object contextView)
		{
			if (contextView is SupportView)
				return contextView as IViewStateWatcher;
			else
			{
				_logger.Warn ("contextView is not a SupportType type. Cant use the TestSupportViewStateWatcherExtension");
				return null;
			}
		}
	}
}


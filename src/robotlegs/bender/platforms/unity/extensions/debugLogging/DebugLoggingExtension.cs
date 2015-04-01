//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.framework.api;
using robotlegs.bender.platforms.unity.extensions.debugLogging.impl;

namespace robotlegs.bender.platforms.unity.extensions.debugLogging
{
	public class DebugLoggingExtension : IExtension
	{
		public void Extend (IContext context)
		{
			context.AddLogTarget(new DebugLogTarget(context));
		}
	}
}


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.extensions.enhancedLogging.impl;
using robotlegs.bender.framework.api;


namespace robotlegs.bender.extensions.enhancedLogging
{
	public class ConsoleLoggingExtension : IExtension
	{
		public void Extend (IContext context)
		{
			context.AddLogTarget(new ConsoleLogTarget(context));
		}
	}
}


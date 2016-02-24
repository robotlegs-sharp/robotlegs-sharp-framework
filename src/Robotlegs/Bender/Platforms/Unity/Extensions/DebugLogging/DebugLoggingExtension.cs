//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Platforms.Unity.Extensions.DebugLogging.Impl;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.DebugLogging
{
	public class DebugLoggingExtension : IExtension
	{
		public void Extend (IContext context)
		{
			context.AddLogTarget(new DebugLogTarget(context));
		}
	}
}


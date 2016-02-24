//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.EnhancedLogging.Impl;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.EnhancedLogging
{
	public class InjectableLoggerExtension : IExtension
	{
		public void Extend (IContext context)
		{
			context.injector.Map(typeof(ILogging)).ToProvider(new LoggerProvider(context));
		}
	}
}


using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.enhancedLogging.impl;

namespace robotlegs.bender.extensions.enhancedLogging
{
	public class InjectableLoggerExtension : IExtension
	{
		public void Extend (IContext context)
		{
			context.injector.Map(typeof(ILogger)).ToProvider(new LoggerProvider(context));
		}
	}
}


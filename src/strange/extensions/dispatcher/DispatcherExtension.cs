using System;
using robotlegs.bender.framework.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;

namespace strange.extensions.dispatcher
{
	public class DispatcherExtension : IExtension
	{
		public void Extend(IContext context)
		{
			context.injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>();
		}
	}
}


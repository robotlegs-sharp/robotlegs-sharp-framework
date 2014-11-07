using System;
using robotlegs.bender.framework.api;
using strange.extensions.injector.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.framework.context.api;

namespace stange.extensions.dispatcher
{
	public class ContextDispatcherConfig : IConfig
	{
		[Inject]
		public IInjectionBinder injectionBinder { get; set; }

		public void Configure()
		{
			injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>().ToSingleton().ToName(ContextKeys.CONTEXT_DISPATCHER);
		}
	}
}


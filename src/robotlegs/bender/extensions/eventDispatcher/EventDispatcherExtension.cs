using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.eventDispatcher
{
	public class EventDispatcherExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext _context;

		private IEventDispatcher _eventDispatcher;

		private LifecycleEventRelay _lifecycleRelay;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
		public EventDispatcherExtension(IEventDispatcher eventDispatcher = null)
		{
			_eventDispatcher = eventDispatcher != null ? eventDispatcher : new EventDispatcher();
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			_context = context;
			_context.injector.Map(typeof(IEventDispatcher)).ToValue(_eventDispatcher);
			_context.BeforeInitializing(ConfigureLifecycleEventRelay);
			_context.AfterDestroying(DestroyLifecycleEventRelay);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void ConfigureLifecycleEventRelay()
		{
			_lifecycleRelay = new LifecycleEventRelay(_context, _eventDispatcher);
		}

		private void DestroyLifecycleEventRelay()
		{
			_lifecycleRelay.Destroy();
		}
	}
}


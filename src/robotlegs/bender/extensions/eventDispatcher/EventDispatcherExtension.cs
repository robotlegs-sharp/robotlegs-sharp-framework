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

//		private IContext _context;

		private IEventDispatcher _eventDispatcher;

//		private LifecycleEventRelay _lifecycleRelay;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
		//TODO: Implement the old constructor when the injection binder can return null
//		public EventDispatcherExtension(IEventDispatcher eventDispatcher = null)
//		{
//			_eventDispatcher = eventDispatcher != null ? eventDispatcher : new EventDispatcher();
//		}
		public EventDispatcherExtension()
		{
			_eventDispatcher = new EventDispatcher();
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
//			_context = context;
			context.injectionBinder.Bind(typeof(IEventDispatcher)).To(_eventDispatcher);
//			_context.injector.map(IEventDispatcher).toValue(_eventDispatcher);
//			_context.beforeInitializing(configureLifecycleEventRelay);
//			_context.afterDestroying(destroyLifecycleEventRelay);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void ConfigureLifecycleEventRelay()
		{
//			_lifecycleRelay = new LifecycleEventRelay(_context, _eventDispatcher);
		}

		private void DestroyLifecycleEventRelay()
		{
//			_lifecycleRelay.Destroy();
		}
	}
}


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.EventManagement
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
			_eventDispatcher = eventDispatcher != null ? eventDispatcher : new Impl.EventDispatcher();
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


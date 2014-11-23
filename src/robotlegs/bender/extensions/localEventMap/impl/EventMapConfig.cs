using System;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.localEventMap.impl
{
	public class EventMapConfig
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IEventDispatcher _dispatcher;

		private Enum _type;

		private Delegate _listener;

		private Delegate _callback;

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public IEventDispatcher dispatcher
		{
			get
			{
				return dispatcher;
			}
		}

		public Enum type
		{
			get 
			{
				return _type;
			}
		}

		public Delegate listener
		{
			get 
			{
				return _listener;
			}
		}

		public Delegate callback
		{
			get 
			{
				return _callback;
			}
		}

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public EventMapConfig (IEventDispatcher dispatcher, Enum type, Delegate listener, Delegate callback)
		{
			_dispatcher = dispatcher;
			_type = type;
			_listener = listener;
			_callback = callback;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public bool Equals (IEventDispatcher dispatcher, Enum type, Delegate listener)
		{
			return this.dispatcher == dispatcher && this.type == type && this.listener == listener;
		}
	}
}


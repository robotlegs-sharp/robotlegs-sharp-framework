using System;

namespace robotlegs.bender.extensions.localEventMap.impl
{
	public class EventMapConfig
	{
		/*============================================================================*/
		/* Private Properties                                                          */
		/*============================================================================*/

		private Enum _type;

		private Delegate _listener;

		private Delegate _callback;

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

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

		public EventMapConfig (Enum type, Delegate listener, Delegate callback)
		{
			_type = type;
			_listener = listener;
			_callback = callback;
		}
	}
}


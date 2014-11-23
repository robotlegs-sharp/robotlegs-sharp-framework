using System;
using robotlegs.bender.extensions.localEventMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.localEventMap.impl
{
	public class EventMap : IEventMap
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private List<EventMapConfig> _listeners = new List<EventMapConfig>();

		private List<EventMapConfig> _suspendedListeners = new List<EventMapConfig>();

		private bool _suspended = false;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void MapListener(IEventDispatcher dispatcher, Enum type, Delegate listener)
		{
			List<EventMapConfig> currentListeners = _suspended ? _suspendedListeners : _listeners;

			EventMapConfig config;

			int i = currentListeners.Count;

			while (i-- > 0) 
			{
				config = currentListeners [i];
				//TODO: Work out this line?
//				if (config.Equals (dispatcher, type, listener)) 
//				{
//					return;
//				}
			}

			// Callback

			Delegate callback = null;

			config = new EventMapConfig (type, listener, callback);
			callback = listener;

			currentListeners.Add (config);

			if (!_suspended) 
			{
				dispatcher.AddEventListener (type, listener);
			}
		}

		public void UnmapListener(IEventDispatcher dispatcher, Enum type, Delegate listener)
		{

		}

		public void UnmapListeners ()
		{

		}

		public void Suspend ()
		{

		}

		public void Resume ()
		{

		}
	}
}


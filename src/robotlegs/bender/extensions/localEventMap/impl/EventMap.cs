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
				if (config.Equals (dispatcher, type, listener))
					return;
			}

			// Callback
			Delegate callback = null;

			callback = listener;
			config = new EventMapConfig (dispatcher, type, listener, callback);

			currentListeners.Add (config);

			if (!_suspended) 
			{
				dispatcher.AddEventListener (type, listener);
			}
		}

		public void UnmapListener(IEventDispatcher dispatcher, Enum type, Delegate listener)
		{
			List<EventMapConfig> currentListeners = _suspended ? _suspendedListeners : _listeners;

			EventMapConfig config;

			int i = currentListeners.Count;

			while (i-- > 0) 
			{
				config = currentListeners [i];
				if (config.Equals (dispatcher, type, listener)) 
				{
					if (!_suspended) 
					{
						dispatcher.RemoveEventListener (type, listener);
					}
					currentListeners.RemoveAt (i);
					return;
				}
			}
		}

		public void UnmapListeners ()
		{
			List<EventMapConfig> currentListeners = _suspended ? _suspendedListeners : _listeners;

			foreach (EventMapConfig config in currentListeners)
			{
				if (!_suspended)
				{
					config.dispatcher.RemoveEventListener (config.type, config.listener);
				}
			}

			currentListeners.Clear ();
		}

		public void Suspend ()
		{
			if (_suspended)
				return;

			_suspended = true;

			EventMapConfig config;
			while (_listeners.Count > 0) 
			{
				config = _listeners[0];
				_listeners.RemoveAt (0);
				config.dispatcher.RemoveEventListener (config.type, config.callback);
				_suspendedListeners.Add (config);
			}
		}

		public void Resume ()
		{
			if (!_suspended)
				return;

			_suspended = false;

			EventMapConfig config;

			while (_suspendedListeners.Count == 0)
			{
				config = _suspendedListeners[0];
				_suspendedListeners.RemoveAt (0);
				config.dispatcher.AddEventListener(config.type, config.listener);
				_listeners.Add(config);
			}
		}
	}
}


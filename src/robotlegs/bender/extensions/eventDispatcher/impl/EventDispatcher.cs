using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using System.Collections.Generic;
using System.Reflection;

namespace robotlegs.bender.extensions.eventDispatcher.impl
{
	public class EventDispatcher : IEventDispatcher
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

//		private Dictionary<Enum, EventData> listeners = new Dictionary<Enum, EventData> ();
		private Dictionary<Enum, List<Delegate>> listeners = new Dictionary<Enum, List<Delegate>> ();

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void AddEventListener<T>(Enum type, Action<T> listener)
		{
			AddEventListener(type, listener as Delegate);
		}

		public void AddEventListener(Enum type, Action<IEvent> listener)
		{
			AddEventListener(type, listener as Delegate);
		}

		public void AddEventListener(Enum type, Action listener)
		{
			AddEventListener(type, listener as Delegate);
		}

		public void AddEventListener(Enum type, Delegate listener)
		{
			if (!listeners.ContainsKey (type))
				listeners.Add (type, new List<Delegate> ());
			listeners[type].Add (listener);
		}

		public void RemoveEventListener<T>(Enum type, Action<T> listener)
		{
			RemoveEventListener(type, listener as Delegate);
		}

		public void RemoveEventListener(Enum type, Action<IEvent> listener)
		{
			RemoveEventListener(type, listener as Delegate);
		}

		public void RemoveEventListener(Enum type, Action listener)
		{
			RemoveEventListener(type, listener as Delegate);
		}

		public void RemoveEventListener(Enum type, Delegate listener)
		{
			if (listeners.ContainsKey (type)) 
			{
				List<Delegate> typeListeners = listeners[type];
				typeListeners.Remove(listener);
				if (typeListeners.Count == 0)
					listeners.Remove (type);
			}
		}

		public bool HasEventListener(Enum type)
		{
			return listeners.ContainsKey (type);
		}

		public void Dispatch (IEvent evt) 
		{
			if (listeners.ContainsKey (evt.type)) 
			{
				// Clone the list, for removing listeners from this dispatch
				Delegate[] typeListeners = listeners[evt.type].ToArray();
				foreach (Delegate listener in typeListeners) 
				{
					//TODO: Make this better, by storing that it's blank, not by checking every time
					if (listener.Method.GetParameters().Length == 0)
						listener.DynamicInvoke(null);
					else
						listener.DynamicInvoke (new object[]{ evt });
				}
			}
		}
	}
}


using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using System.Collections.Generic;
using System.Reflection;

namespace robotlegs.bender.extensions.eventDispatcher.impl
{
	public class EventDispatcher : IEventDispatcher
	{
//		private Dictionary<Enum, EventData> listeners = new Dictionary<Enum, EventData> ();
		private Dictionary<Enum, List<Delegate>> listeners = new Dictionary<Enum, List<Delegate>> ();

		public EventDispatcher ()
		{

		}

		public void AddEventListener<T>(Enum type, Action<T> listener)
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

		public void Dispatch (IEvent evt) 
		{
			if (listeners.ContainsKey (evt.type)) 
			{
				foreach (Delegate listener in listeners[evt.type]) 
				{
					listener.DynamicInvoke (new object[]{ evt });
				}
			}
		}
	}
}


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.EventManagement.API;

namespace Robotlegs.Bender.Extensions.EventManagement.Impl
{
	public class EventDispatcher : IEventDispatcher
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

//		private Dictionary<Enum, EventData> listeners = new Dictionary<Enum, EventData> ();
		private Dictionary<Enum, List<Delegate>> _listeners = new Dictionary<Enum, List<Delegate>> ();

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public virtual void AddEventListener<T>(Enum type, Action<T> listener)
		{
			AddEventListener(type, listener as Delegate);
		}

		public virtual void AddEventListener(Enum type, Action<IEvent> listener)
		{
			AddEventListener(type, listener as Delegate);
		}

		public virtual void AddEventListener(Enum type, Action listener)
		{
			AddEventListener(type, listener as Delegate);
		}

		public virtual void AddEventListener(Enum type, Delegate listener)
		{
			if (!_listeners.ContainsKey (type))
				_listeners.Add (type, new List<Delegate> ());
			_listeners[type].Add (listener);
		}

		public virtual void RemoveEventListener<T>(Enum type, Action<T> listener)
		{
			RemoveEventListener(type, listener as Delegate);
		}

		public virtual void RemoveEventListener(Enum type, Action<IEvent> listener)
		{
			RemoveEventListener(type, listener as Delegate);
		}

		public virtual void RemoveEventListener(Enum type, Action listener)
		{
			RemoveEventListener(type, listener as Delegate);
		}

		public virtual void RemoveEventListener(Enum type, Delegate listener)
		{
			if (_listeners.ContainsKey (type)) 
			{
				List<Delegate> typeListeners = _listeners[type];
				typeListeners.Remove(listener);
				if (typeListeners.Count == 0)
					_listeners.Remove (type);
			}
		}

		public virtual bool HasEventListener(Enum type)
		{
			return _listeners.ContainsKey (type);
		}

		public virtual void RemoveAllEventListeners()
		{
			_listeners.Clear ();
		}

		public virtual void Dispatch (IEvent evt) 
		{
			if (_listeners.ContainsKey (evt.type)) 
			{
				// Clone the list, for removing listeners from this dispatch
				Delegate[] typeListeners = _listeners[evt.type].ToArray();
				foreach (Delegate listener in typeListeners) 
				{
					//TODO: Do not dynamic invoke if it is an Action, as it's much much slower to dynamic invoke
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


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.eventDispatcher.impl;
using robotlegs.bender.extensions.localEventMap.api;

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

		public void MapListener(IEventDispatcher dispatcher, Enum type, Delegate listener, Type eventClass = null)
		{
			if (eventClass == null)
			{	
				eventClass = typeof(Event);
			}

			List<EventMapConfig> currentListeners = _suspended ? _suspendedListeners : _listeners;

			EventMapConfig config;

			int i = currentListeners.Count;

			while (i-- > 0) 
			{
				config = currentListeners [i];
				if (config.Equals (dispatcher, type, listener, eventClass))
					return;
			}

			Delegate callback = eventClass == typeof(Event) ? listener : (Action<IEvent>)delegate(IEvent evt){
				RouteEventToListener(evt, listener, eventClass);
			};

			config = new EventMapConfig (dispatcher, type, listener, eventClass, callback);

			currentListeners.Add (config);

			if (!_suspended) 
			{
				dispatcher.AddEventListener (type, callback);
			}
		}

		public void UnmapListener(IEventDispatcher dispatcher, Enum type, Delegate listener, Type eventClass = null)
		{
			if (eventClass == null)
			{	
				eventClass = typeof(Event);
			}

			List<EventMapConfig> currentListeners = _suspended ? _suspendedListeners : _listeners;

			EventMapConfig config;

			int i = currentListeners.Count;

			while (i-- > 0) 
			{
				config = currentListeners [i];
				if (config.Equals (dispatcher, type, listener, eventClass)) 
				{
					if (!_suspended) 
					{
						dispatcher.RemoveEventListener (type, config.callback);
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
					config.dispatcher.RemoveEventListener (config.type, config.callback);
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

			while (_suspendedListeners.Count > 0)
			{
				config = _suspendedListeners[0];
				_suspendedListeners.RemoveAt (0);
				config.dispatcher.AddEventListener(config.type, config.callback);
				_listeners.Add(config);
			}
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		/**
		* Event Handler
		*
		* @param event The <code>Event</code>
		* @param listener
		* @param originalEventClass
		*/
		protected void RouteEventToListener(IEvent evt, Delegate listener, Type originalEventClass)
		{
//			if (evt is originalEventClass)
			if (originalEventClass.IsInstanceOfType(evt))
			{
				listener.DynamicInvoke(new object[]{evt});
			}
		}
	}
}


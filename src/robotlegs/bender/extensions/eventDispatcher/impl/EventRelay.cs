//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.eventDispatcher.impl
{
	/// <summary>
	/// Relays events from a source to a destination
	/// </summary>
	public class EventRelay
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IEventDispatcher _source;

		private IEventDispatcher _destination;

		private List<Enum> _types;

		private bool _active;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/// <summary>
		/// Relays events from the source to the destination
		/// </summary>
		/// <param name="source">Source Event Dispatcher</param>
		/// <param name="destination">Destination Event Dispatcher</param>
		/// <param name="types">The list of event types to relay</param>
		public EventRelay(IEventDispatcher source, IEventDispatcher destination, IEnumerable<Enum> types = null)
		{
			_source = source;
			_destination = destination;
			_types = types == null ? new List<Enum>() : new List<Enum>(types);
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		/// <summary>
		/// Start relaying events
		/// </summary>
		public EventRelay Start()
		{
			if (!_active)
			{
				_active = true;
				AddListeners();
			}
			return this;
		}

		/// <summary>
		/// Stop relaying events.
		/// </summary>
		public EventRelay Stop()
		{
			if (_active)
			{
				_active = false;
				RemoveListeners();
			}
			return this;
		}

		/// <summary>
		/// Ass a new event type to relay
		/// </summary>
		/// <param name="eventType">The event type to add</param>
		public void AddType(Enum eventType)
		{
			_types.Add(eventType);
			if (_active)
			{
				AddListener(eventType);
			}
		}

		/// <summary>
		/// Remove a relay event type
		/// </summary>
		/// <param name="eventType">The event type to remove</param>
		public void RemoveType(Enum eventType)
		{
			int index = _types.IndexOf(eventType);
			if (index > -1)
			{
				_types.RemoveAt (index);
				RemoveListener(eventType);
			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void RemoveListener(Enum type)
		{
			_source.RemoveEventListener(type, _destination.Dispatch);
		}

		private void AddListener(Enum type)
		{
			_source.AddEventListener(type, _destination.Dispatch);
		}

		private void AddListeners()
		{
			foreach (Enum type in _types)
			{
				AddListener(type);
			}
		}

		private void RemoveListeners()
		{
			foreach (Enum type in _types)
			{
				RemoveListener(type);
			}
		}
	}
}

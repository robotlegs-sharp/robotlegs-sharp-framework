//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
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

		private Type _eventClass;

		private Delegate _callback;

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public IEventDispatcher dispatcher
		{
			get
			{
				return _dispatcher;
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

		public Type eventClass
		{
			get 
			{
				return _eventClass;
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

		public EventMapConfig (IEventDispatcher dispatcher, Enum type, Delegate listener, Type eventClass, Delegate callback)
		{
			_dispatcher = dispatcher;
			_type = type;
			_eventClass = eventClass;
			_listener = listener;
			_callback = callback;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public bool Equals (IEventDispatcher dispatcher, Enum type, Delegate listener, Type eventClass)
		{
			Console.WriteLine (_dispatcher == dispatcher);
			Console.WriteLine (_type == type);
			Console.WriteLine (_listener == listener);
			Console.WriteLine (_eventClass == eventClass);
			return _dispatcher == dispatcher && _type.Equals(type) && _listener == listener && _eventClass == eventClass;
		}
	}
}


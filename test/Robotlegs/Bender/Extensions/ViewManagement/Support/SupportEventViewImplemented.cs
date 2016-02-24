//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.EventManagement.Impl;

namespace Robotlegs.Bender.Extensions.ViewManagement.Support
{
	public class SupportEventViewImplemented : SupportView, IEventDispatcher
	{
		private IEventDispatcher _dispatcher = new EventDispatcher();

		public void AddEventListener<T> (Enum type, Action<T> listener)
		{
			_dispatcher.AddEventListener<T> (type, listener);
		}

		public void AddEventListener (Enum type, Action<IEvent> listener)
		{
			_dispatcher.AddEventListener(type, listener);
		}

		public void AddEventListener (Enum type, Action listener)
		{
			_dispatcher.AddEventListener(type, listener);
		}

		public void AddEventListener (Enum type, Delegate listener)
		{
			_dispatcher.AddEventListener(type, listener);
		}

		public void RemoveEventListener<T> (Enum type, Action<T> listener)
		{
			_dispatcher.RemoveEventListener(type, listener);
		}

		public void RemoveEventListener (Enum type, Action<IEvent> listener)
		{
			_dispatcher.RemoveEventListener (type, listener);
		}

		public void RemoveEventListener (Enum type, Action listener)
		{
			_dispatcher.RemoveEventListener (type, listener);
		}

		public void RemoveEventListener (Enum type, Delegate listener)
		{
			_dispatcher.RemoveEventListener (type, listener);
		}

		public void RemoveAllEventListeners ()
		{
			_dispatcher.RemoveAllEventListeners ();
		}

		public bool HasEventListener (Enum type)
		{
			return _dispatcher.HasEventListener (type);
		}

		public void Dispatch (IEvent evt)
		{
			_dispatcher.Dispatch (evt);
		}
	}
}


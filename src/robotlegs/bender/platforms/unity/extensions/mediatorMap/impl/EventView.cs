//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.platforms.unity.extensions.mediatorMap.impl
{
	public class EventView : View
	{
		private IEventDispatcher _dispatcher = new EventDispatcher();
		public IEventDispatcher dispatcher
		{
			get
			{
				return _dispatcher;
			}
			set
			{
				_dispatcher = value;
			}
		}
	}
}


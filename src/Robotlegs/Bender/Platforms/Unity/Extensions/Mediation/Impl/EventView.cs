//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.EventManagement.Impl;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.Mediation.Impl
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


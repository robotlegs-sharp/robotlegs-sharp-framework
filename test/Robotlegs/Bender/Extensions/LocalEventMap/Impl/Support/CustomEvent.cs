//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using Robotlegs.Bender.Extensions.EventManagement.Impl;

namespace Robotlegs.Bender.Extensions.LocalEventMap.Impl.Support
{
	public class CustomEvent : Event
	{
		public enum Type
		{
			STARTED
		}

		public CustomEvent(Type type) : base(type)
		{
			
		}
	}
}
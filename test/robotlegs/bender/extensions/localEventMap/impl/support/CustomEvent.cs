//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//  Copyright (c) 2011 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.localEventMap.impl.support
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
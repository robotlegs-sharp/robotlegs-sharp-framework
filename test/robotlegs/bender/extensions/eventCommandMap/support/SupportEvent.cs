//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using robotlegs.bender.extensions.eventDispatcher.api;
using System;


namespace robotlegs.bender.extensions.eventCommandMap.support
{
	public class SupportEvent : IEvent
	{
		public enum Type
		{
			TYPE1,
			TYPE2
		}
		private SupportEvent.Type _type;

		public SupportEvent (SupportEvent.Type type)
		{
			_type = type;
		}

		public Enum type
		{
			get { return _type; }
		}
	}
}


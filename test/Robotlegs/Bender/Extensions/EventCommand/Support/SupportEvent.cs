//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using Robotlegs.Bender.Extensions.EventManagement.API;
using System;


namespace Robotlegs.Bender.Extensions.EventCommand.Support
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


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.EventManagement.API;

namespace Robotlegs.Bender.Extensions.EventManagement.Impl
{
	[Serializable]
	public class Event : IEvent
	{
		private Enum _type;

		public Enum type 
		{
			get 
			{
				return _type;
			}
		}

		public Event (Enum type)
		{
			_type = type;
		}
	}
}


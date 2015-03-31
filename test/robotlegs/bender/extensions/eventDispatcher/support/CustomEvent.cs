//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.eventDispatcher.support
{
	public class CustomEvent : Event
	{
		public enum Type
		{
			A,
			B,
			C
		}

		public string message;

		public CustomEvent (Type type, string message) : base(type)
		{
			this.message = message;
		}
	}
}


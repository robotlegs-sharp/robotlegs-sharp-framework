//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.EventManagement.Impl;

namespace Robotlegs.Bender.Extensions.EventManagement.Support
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


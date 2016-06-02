//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace Robotlegs.Bender.Extensions.CommandCenter.Support
{
	public class CallbackCommand
	{
		[Inject("ExecuteCallback")]
		public Action callback;

		public void Execute()
		{
			callback ();
		}
	}
}


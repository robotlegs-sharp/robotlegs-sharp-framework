//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace Robotlegs.Bender.Framework.API
{
	public interface ILifecycleEvent
	{
		event Action<Exception> ERROR;

		event Action STATE_CHANGE;

		event Action<object> PRE_INITIALIZE;
		event Action<object> INITIALIZE;
		event Action<object> POST_INITIALIZE;

		event Action<object> PRE_SUSPEND;
		event Action<object> SUSPEND;
		event Action<object> POST_SUSPEND;

		event Action<object> PRE_RESUME;
		event Action<object> RESUME;
		event Action<object> POST_RESUME;

		event Action<object> PRE_DESTROY;
		event Action<object> DESTROY;
		event Action<object> POST_DESTROY;
	}
}


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.localEventMap.api
{
	public interface IEventMap
	{
		void MapListener(IEventDispatcher dispatcher, Enum type, Delegate listener, Type eventClass = null);

		void UnmapListener(IEventDispatcher dispatcher, Enum type, Delegate listener, Type eventClass = null);

		void UnmapListeners();

		void Suspend();

		void Resume();
	}
}


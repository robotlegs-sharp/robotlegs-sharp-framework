//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.eventDispatcher.impl;
using robotlegs.bender.extensions.eventCommandMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.eventCommandMap.support
{
	public class CommandUnmappingCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IEvent evt;

		[Inject("nestedCommand")]
		public Type commandClass;

		[Inject]
		public IEventCommandMap eventCommandMap;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			eventCommandMap.Unmap(evt.type, typeof(Event)).FromCommand(commandClass);
		}
	}
}


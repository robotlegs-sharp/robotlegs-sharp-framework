//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.eventCommandMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.commandCenter.support;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.eventCommandMap.support
{
	public class CascadingCommand
	{
		/*============================================================================*/
		/* Public Static Properties                                                   */
		/*============================================================================*/

		public enum EventType
		{
			CASCADING_EVENT
		}

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IEventDispatcher dispatcher;

		[Inject]
		public IEventCommandMap eventCommandMap;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			eventCommandMap
				.Map(EventType.CASCADING_EVENT)
				.ToCommand<NullCommand>().Once();

			dispatcher.Dispatch(new Event(EventType.CASCADING_EVENT));
		}
	}
}


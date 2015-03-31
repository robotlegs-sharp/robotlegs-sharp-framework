//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.commandCenter.dsl;
using robotlegs.bender.extensions.commandCenter.impl;

namespace robotlegs.bender.extensions.eventCommandMap.api
{
	public interface IEventCommandMap
	{
		/// <summary>
		/// Creates a mapping for an Event based trigger
		/// </summary>
		/// <param name="type">The Event type</param>
		/// <param name="eventClass">The concrete Event class</param>
		ICommandMapper Map(Enum type, Type eventClass = null);
		ICommandMapper Map<T>(Enum type);

		/// <summary>
		/// Unmaps an Event based trigger from a Command
		/// </summary>
		/// <param name="type">The Event type</param>
		/// <param name="eventClass">The concrete Event class</param>
		ICommandUnmapper Unmap(Enum type, Type eventClass = null);
		ICommandUnmapper Unmap<T>(Enum type);

		/// <summary>
		/// Adds a handler to a process mappings
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="handler">Function that accepts a mapping</param>
		IEventCommandMap AddMappingProcessor(CommandMappingList.Processor handler);
	}
}


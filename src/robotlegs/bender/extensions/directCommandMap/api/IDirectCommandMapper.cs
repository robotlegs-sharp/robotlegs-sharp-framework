//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.commandCenter.api;

namespace robotlegs.bender.extensions.directCommandMap.api
{
	public interface IDirectCommandMapper
	{
		/// <summary>
		/// Creates a mapping for a command class
		/// </summary>
		/// <returns>The command configurator</returns>
		/// <param name="commandClass">The concrete command class</param>
		IDirectCommandConfigurator Map(Type commandClass);
		IDirectCommandConfigurator Map<T>();

		/// <summary>
		/// Executes the configured command(s)
		/// </summary>
		/// <param name="payload">The command payload</param>
		void Execute(CommandPayload payload = null);
	}
}


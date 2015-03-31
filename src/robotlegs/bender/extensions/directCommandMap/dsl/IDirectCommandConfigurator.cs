//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace robotlegs.bender.extensions.directCommandMap.api
{
	public interface IDirectCommandConfigurator : IDirectCommandMapper
	{
		/// <summary>
		/// The 'execute' method to invoke on the Command instance
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="name">The method name</param>
		IDirectCommandConfigurator WithExecuteMethod(string name);

		/// <summary>
		/// Guards to check before allowing a command to execute
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="guards">Guards</param>
		IDirectCommandConfigurator WithGuards(params object[] guards);

		/// <summary>
		/// Hooks to run before command execution
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="hooks">Hooks</param>
		IDirectCommandConfigurator WithHooks(params object[] hooks);

		/// <summary>
		/// Should the payload values be injected into the command instance?
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="value">Toggle</param>
		IDirectCommandConfigurator WithPayloadInjection(bool value = true);
	}
}


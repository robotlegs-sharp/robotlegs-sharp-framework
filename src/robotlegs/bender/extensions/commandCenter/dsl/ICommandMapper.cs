//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace Robotlegs.Bender.Extensions.CommandCenter.DSL
{
	public interface ICommandMapper
	{
		/// <summary>
		/// Creates a command mapping
		/// </summary>
		/// <returns>Mapping configurator</returns>
		/// <param name="commandType">The Command Class to map</param>
		ICommandConfigurator ToCommand<T>();
		ICommandConfigurator ToCommand(Type commandType);
	}
}
//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System.Collections.Generic;

namespace Robotlegs.Bender.Extensions.CommandCenter.API
{
	/// <summary>
	/// Optional Command interface.
	///
	/// <p>Note, you do not need to implement this interface,
	/// any class with an execute method can be used.</p>
	/// </summary>
	public interface ICommandExecutor
	{
		/// <summary>
		/// Execute a command for a given mapping
		/// </summary>
		/// <param name="mapping">The Command Mapping</param>
		/// <param name="payload">The Command Payload</param>
		void ExecuteCommand(ICommandMapping mapping, CommandPayload payload);

		/// <summary>
		/// Execute a list of commands for a given list of mappings
		/// </summary>
		/// <param name="mapping">The Command Mappings</param>
		/// <param name="payload">The Command Payload</param>
		void ExecuteCommands(IEnumerable<ICommandMapping> mapping, CommandPayload payload);
	}
}
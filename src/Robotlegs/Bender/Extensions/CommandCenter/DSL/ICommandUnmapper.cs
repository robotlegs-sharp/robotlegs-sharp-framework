//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;

namespace Robotlegs.Bender.Extensions.CommandCenter.DSL
{
	public interface ICommandUnmapper
	{
		/// <summary>
		/// Unmaps a Command
		/// </summary>
		/// <param name="commandType">Command to unmap</param>
		void FromCommand<T>();
		void FromCommand(Type commandType);

		/// <summary>
		/// Unmaps all commands from this trigger
		/// </summary>
		void FromAll();
	}
}

﻿//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.DSL;
using System;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.API
{
    public interface IDirectAsyncCommandMapper
    {
        /// <summary>
		/// Creates a mapping for a command class
		/// </summary>
		/// <returns>The command configurator</returns>
		/// <param name="commandClass">The concrete command class</param>
        IDirectAsyncCommandConfigurator Map<T>() where T : IAsyncCommand;

        /// <summary>
        /// Executes the configured command(s)
        /// </summary>
        /// <param name="payload">The command payload</param>
        void Execute(CommandPayload payload = null);
    }
}

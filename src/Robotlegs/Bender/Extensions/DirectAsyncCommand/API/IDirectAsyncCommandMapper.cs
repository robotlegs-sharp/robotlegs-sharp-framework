//------------------------------------------------------------------------------
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
        /// Executes the configured command(s)
        /// </summary>
        /// <param name="payload">The command payload</param>
        void Execute(CommandPayload payload = null);

        /// <summary>
        /// Creates a mapping for a command class.
        /// </summary>
        /// <typeparam name="T">The concrete asynchronous command class.</typeparam>
        /// <returns>The command configurator.</returns>
        IDirectAsyncCommandConfigurator Map<T>() where T : IAsyncCommand;

        /// <summary>
        /// Sets the callback function that remaining commands was aborted.
        /// </summary>
        /// <param name="callback">The callback function that remaining commands was aborted.</param>
        /// <returns>The command mapper.</returns>
        IDirectAsyncCommandMapper SetCommandsAbortedCallback(Action callback);

        /// <summary>
        /// Sets the callback function that all commands executed.
        /// </summary>
        /// <param name="callback">The callback function that all commands executed.</param>
        /// <returns>The command mapper.</returns>
        IDirectAsyncCommandMapper SetCommandsExecutedCallback(Action callback);
    }
}
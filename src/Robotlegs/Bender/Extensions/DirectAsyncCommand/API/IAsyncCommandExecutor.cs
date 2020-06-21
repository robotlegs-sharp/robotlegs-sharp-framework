//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.CommandCenter.API;
using System;
using System.Collections.Generic;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.API
{
    /// <summary>
	/// Optional asynchronous Command interface.
	/// </summary>
    public interface IAsyncCommandExecutor
    {
        /// <summary>
        /// Gets a value indicating whether the asynchronous commands execution is aborted.
        /// </summary>
        /// <value><c>true</c> if the asynchronous commands is aborted; otherwise, <c>false</c>.</value>
        bool IsAborted
        {
            get;
        }

        /// <summary>
        /// Aborts asynchronous Command execution.
        /// </summary>
        /// <param name="abortCurrentCommand">if set to <c>true</c> abort current command execution.</param>
        void Abort(bool abortCurrentCommand = true);

        /// <summary>
		/// Execute a list of asynchronous commands for a given list of mappings
		/// </summary>
		/// <param name="mapping">The Command Mappings</param>
		/// <param name="payload">The Command Payload</param>
        void ExecuteAsyncCommands(IEnumerable<ICommandMapping> mapping, CommandPayload payload);

        /// <summary>
        /// Sets the callback function that remaining commands was aborted.
        /// </summary>
        /// <param name="callback">The callback function that remaining commands was aborted.</param>
        void SetCommandsAbortedCallback(Action callback);

        /// <summary>
        /// Sets the callback function that all commands executed.
        /// </summary>
        /// <param name="callback">The callback function that all commands executed to invoke.</param>
        void SetCommandsExecutedCallback(Action callback);
    }
}
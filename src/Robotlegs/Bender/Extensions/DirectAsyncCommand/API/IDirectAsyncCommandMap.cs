//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.CommandCenter.Impl;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.API
{
    public interface IDirectAsyncCommandMap : IDirectAsyncCommandMapper
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
        /// Aborts the asynchronous commands execution.
        /// </summary>
        /// <param name="abortCurrentCommand">if set to <c>true</c> abort current command execution.</param>
        void Abort(bool abortCurrentCommand = true);

        /// <summary>
		/// Adds a handler to the process mappings
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="handler">Delegate that accepts a mapping</param>
		IDirectAsyncCommandMap AddMappingProcessor(CommandMappingList.Processor handler);
    }
}

//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Extensions.CommandCenter.Impl;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;
using Robotlegs.Bender.Framework.API;
using System;
using System.Collections.Generic;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.Impl
{
    internal class AsyncCommandExecutor : IAsyncCommandExecutor
    {
        private const string AsyncCommandExecutedCallbackName = "AsyncCommandExecutedCallback";

        /*============================================================================*/
        /* Private Fields                                                         */
        /*============================================================================*/

        private IContext _context;

        private Queue<ICommandMapping> _commandMappingQueue;

        private CommandPayload _payload;

        private ICommandExecutor _commandExecutor;

        private CommandExecutor.HandleResultDelegate _handleResult;

        private IAsyncCommand _currentAsyncCommand;

        private Action _commandsExecutedCallback;

        private Action _commandsAbortedCallback;

        /*============================================================================*/
        /* Public Properties                                                         */
        /*============================================================================*/

        public bool IsAborted
        {
            get;
            private set;
        }

        /*============================================================================*/
        /* Constructors                                                           */
        /*============================================================================*/

        public AsyncCommandExecutor(IContext context, IInjector injector,
            CommandExecutor.RemoveMappingDelegate removeMapping = null, CommandExecutor.HandleResultDelegate handleResult = null)
        {
            IsAborted = false;
            _context = context;
            _handleResult = handleResult;
            _commandExecutor = new CommandExecutor(injector, removeMapping, HandleCommandExecuteResult);
        }

        /*============================================================================*/
        /* Public Functions                                                           */
        /*============================================================================*/

        /// <summary>
        /// Aborts asynchronous Command execution.
        /// </summary>
        /// <param name="abortCurrentCommand">if set to <c>true</c> abort current command execution.</param>
        public void Abort(bool abortCurrentCommand = true)
        {
            IsAborted = true;

            if (abortCurrentCommand && _currentAsyncCommand != null)
            {
                _currentAsyncCommand.Abort();
            }
        }

        public void ExecuteAsyncCommands(IEnumerable<ICommandMapping> mappings, CommandPayload payload)
        {
            _commandMappingQueue = new Queue<ICommandMapping>(mappings);
            _payload = payload;
            ExecuteNextCommand();
        }

        public void SetCommandsExecutedCallback(Action callback)
        {
            _commandsExecutedCallback = callback;
        }

        public void SetCommandsAbortedCallback(Action callback)
        {
            _commandsAbortedCallback = callback;
        }

        /*============================================================================*/
        /* Private Functions                                                           */
        /*============================================================================*/

        private void ExecuteNextCommand()
        {
            while (!IsAborted && _commandMappingQueue.Count > 0)
            {
                ICommandMapping mapping = _commandMappingQueue.Dequeue();

                if (mapping != null)
                {
                    _context.injector.Map<Action<IAsyncCommand, bool>>(AsyncCommandExecutedCallbackName).ToValue((Action<IAsyncCommand, bool>)CommandExecutedCallback);
                    _commandExecutor.ExecuteCommand(mapping, _payload);
                    return;
                }
            }

            if (IsAborted)
            {
                _commandsAbortedCallback?.Invoke();
            }
            else if (_commandMappingQueue.Count == 0)
            {
                _commandsExecutedCallback?.Invoke();
            }
        }

        private void HandleCommandExecuteResult(object result, object command, ICommandMapping CommandMapping)
        {
            _currentAsyncCommand = command as IAsyncCommand;

            if (_handleResult != null)
                _handleResult.Invoke(result, command, CommandMapping);

            _context.Detain(command);
        }

        private void CommandExecutedCallback(IAsyncCommand command, bool stop = false)
        {
            _context.Release(command);
            _context.injector.Unmap<Action<IAsyncCommand, bool>>(AsyncCommandExecutedCallbackName);

            if (stop)
            {
                Abort(false);
            }

            ExecuteNextCommand();
        }
    }
}

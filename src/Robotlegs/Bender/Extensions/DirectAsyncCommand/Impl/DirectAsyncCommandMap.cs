//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Extensions.CommandCenter.Impl;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.DSL;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.Impl
{
    public class DirectAsyncCommandMap : IDirectAsyncCommandMap
    {
        /*============================================================================*/
        /* Private Fields                                                         */
        /*============================================================================*/

        private List<CommandMappingList.Processor> _mappingProcessors = new List<CommandMappingList.Processor>();

        private IContext _context;

        private IAsyncCommandExecutor _executor;

        private CommandMappingList _mappings;

        /*============================================================================*/
        /* Pulbic Properties                                                         */
        /*============================================================================*/

        public bool IsAborted
        {
            get
            {
                if (_executor != null)
                {
                    return _executor.IsAborted;
                }

                return false;
            }
        }

        /*============================================================================*/
        /* Constructor                                                                */
        /*============================================================================*/

        public DirectAsyncCommandMap(IContext context)
        {
            _context = context;
            IInjector sandboxedInjector = context.injector.CreateChild();
            sandboxedInjector.Map(typeof(IDirectAsyncCommandMap)).ToValue(this);
            _mappings = new CommandMappingList(
                new NullCommandTrigger(), _mappingProcessors, context.GetLogger(this));
            _executor = new AsyncCommandExecutor(_context, sandboxedInjector, _mappings.RemoveMapping);
        }

        /*============================================================================*/
        /* Public Functions                                                           */
        /*============================================================================*/

        public void Abort(bool abortCurrentCommand = true)
        {
            _executor.Abort(abortCurrentCommand);
        }

        public IDirectAsyncCommandMap AddMappingProcessor(CommandMappingList.Processor handler)
        {
            if (!_mappingProcessors.Contains(handler))
                _mappingProcessors.Add(handler);
            return this;
        }

        public void Execute(CommandPayload payload = null)
        {
            _executor.ExecuteAsyncCommands(_mappings.GetList(), payload);
        }

        public IDirectAsyncCommandConfigurator Map<T>() where T : IAsyncCommand
        {
            return new DirectAsyncCommandMapper(_executor, _mappings, typeof(T));
        }

        public IDirectAsyncCommandMapper SetCommandsExecutedCallback(Action callback)
        {
            if (_executor != null)
            {
                _executor.SetCommandsExecutedCallback(callback);
            }

            return this;
        }

        public IDirectAsyncCommandMapper SetCommandsAbortedCallback(Action callback)
        {
            if (_executor != null)
            {
                _executor.SetCommandsAbortedCallback(callback);
            }

            return this;
        }
    }
}

//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Extensions.CommandCenter.Impl;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.DSL;
using System;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.Impl
{
    public class DirectAsyncCommandMapper : IDirectAsyncCommandConfigurator
    {
        /*============================================================================*/
        /* Private Properties                                                         */
        /*============================================================================*/

        private ICommandMappingList _mappings;

        private ICommandMapping _mapping;

        private IAsyncCommandExecutor _executor;

        /*============================================================================*/
        /* Constructor                                                                */
        /*============================================================================*/

        public DirectAsyncCommandMapper(IAsyncCommandExecutor executor, ICommandMappingList mappings, Type commandClass)
        {
            _executor = executor;
            _mappings = mappings;
            _mapping = new CommandMapping(commandClass);
            _mapping.SetFireOnce(true);
            _mappings.AddMapping(_mapping);
        }

        /*============================================================================*/
        /* Public Functions                                                           */
        /*============================================================================*/

        public void Execute(CommandPayload payload = null)
        {
            _executor.ExecuteAsyncCommands(_mappings.GetList(), payload);
        }

        public IDirectAsyncCommandConfigurator Map<T>() where T : IAsyncCommand
        {
            return new DirectAsyncCommandMapper(_executor, _mappings, typeof(T));
        }

        public IDirectAsyncCommandConfigurator WithExecuteMethod(string name)
        {
            _mapping.SetExecuteMethod(name);
            return this;
        }

        public IDirectAsyncCommandConfigurator WithGuards(params object[] guards)
        {
            _mapping.AddGuards(guards);
            return this;
        }

        public IDirectAsyncCommandConfigurator WithHooks(params object[] hooks)
        {
            _mapping.AddHooks(hooks);
            return this;
        }

        public IDirectAsyncCommandConfigurator WithPayloadInjection(bool value = true)
        {
            _mapping.SetPayloadInjectionEnabled(value);
            return this;
        }
    }
}

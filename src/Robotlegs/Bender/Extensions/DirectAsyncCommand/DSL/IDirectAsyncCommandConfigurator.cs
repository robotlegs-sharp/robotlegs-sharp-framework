//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.DSL
{
    public interface IDirectAsyncCommandConfigurator : IDirectAsyncCommandMapper
    {
        /// <summary>
        /// The 'execute' method to invoke on the Command instance
        /// </summary>
        /// <returns>Self</returns>
        /// <param name="name">The method name</param>
        IDirectAsyncCommandConfigurator WithExecuteMethod(string name);

        /// <summary>
        /// Guards to check before allowing a command to execute
        /// </summary>
        /// <returns>Self</returns>
        /// <param name="guards">Guards</param>
        IDirectAsyncCommandConfigurator WithGuards(params object[] guards);

        /// <summary>
        /// Hooks to run before command execution
        /// </summary>
        /// <returns>Self</returns>
        /// <param name="hooks">Hooks</param>
        IDirectAsyncCommandConfigurator WithHooks(params object[] hooks);

        /// <summary>
        /// Should the payload values be injected into the command instance?
        /// </summary>
        /// <returns>Self</returns>
        /// <param name="value">Toggle</param>
        IDirectAsyncCommandConfigurator WithPayloadInjection(bool value = true);
    }
}
//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

namespace Robotlegs.Bender.Extensions.CommandCenter.DSL
{
	public interface ICommandConfigurator
	{
		/**
		 * The "execute" method to invoke on the Command instance
		 * @param name Method name
		 * @return Self
		 */
		ICommandConfigurator WithExecuteMethod(string name);
		
		/**
		 * Guards to check before allowing a command to execute
		 * @param guards Guards
		 * @return Self
		 */
		ICommandConfigurator WithGuards(params object[] guards);
		ICommandConfigurator WithGuards<T>();
		ICommandConfigurator WithGuards<T1, T2>();
		ICommandConfigurator WithGuards<T1, T2, T3>();
		ICommandConfigurator WithGuards<T1, T2, T3, T4>();
		ICommandConfigurator WithGuards<T1, T2, T3, T4, T5>();
		
		/**
		 * Hooks to run before command execution
		 * @param hooks Hooks
		 * @return Self
		 */
		ICommandConfigurator WithHooks(params object[] hooks);
		ICommandConfigurator WithHooks<T>();
		ICommandConfigurator WithHooks<T1, T2>();
		ICommandConfigurator WithHooks<T1, T2, T3>();
		ICommandConfigurator WithHooks<T1, T2, T3, T4>();
		ICommandConfigurator WithHooks<T1, T2, T3, T4, T5>();
		
		/**
		 * Should this command only run once?
		 * @param value Toggle
		 * @return Self
		 */
		ICommandConfigurator Once(bool value = true);
		
		/**
		 * Should the payload values be injected into the command instance?
		 * @param value Toggle
		 * @return Self
		 */
		ICommandConfigurator WithPayloadInjection(bool value);
	}
}


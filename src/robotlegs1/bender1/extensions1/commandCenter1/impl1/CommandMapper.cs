//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Extensions.CommandCenter.DSL;

namespace Robotlegs.Bender.Extensions.CommandCenter.Impl
{
	public class CommandMapper : ICommandMapper, ICommandUnmapper, ICommandConfigurator
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		 
		private ICommandMappingList _mappings;
		
		private ICommandMapping _mapping;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
		
		/**
		 * Creates a Command Mapper
		 * @param mappings The command mapping list to add mappings to
		 */
		public CommandMapper(ICommandMappingList mappings)
		{
			_mappings = mappings;
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/
		
		/**
		 * @inheritDoc
		*/
		public ICommandConfigurator ToCommand<T>()
		{
			return ToCommand (typeof(T));
		}

		public ICommandConfigurator ToCommand(Type commandClass)
		{
			_mapping = new CommandMapping(commandClass);
			_mappings.AddMapping(_mapping);
			return this;
		}
		
		/**
		 * @inheritDoc
		 */
		public void FromCommand<T>()
		{
			FromCommand (typeof(T));
		}

		public void FromCommand(Type commandClass)
		{
			_mappings.RemoveMappingFor(commandClass);
		}
		
		/**
		 * @inheritDoc
		 */
		public void FromAll()
		{
			_mappings.RemoveAllMappings();
		}
		
		/**
		 * @inheritDoc
		 */
		public ICommandConfigurator Once(bool value = true)
		{
			_mapping.SetFireOnce(value);
			return this;
		}
		
		/**
		 * @inheritDoc
		 */
		public ICommandConfigurator WithGuards(params object[] guards)
		{
			_mapping.AddGuards(guards);
			return this;
		}

		public ICommandConfigurator WithGuards<T>()
		{
			return WithGuards (typeof(T));
		}

		public ICommandConfigurator WithGuards<T1, T2>()
		{
			return WithGuards (typeof(T1), typeof(T2));
		}

		public ICommandConfigurator WithGuards<T1, T2, T3>()
		{
			return WithGuards (typeof(T1), typeof(T2), typeof(T3));
		}

		public ICommandConfigurator WithGuards<T1, T2, T3, T4>()
		{
			return WithGuards (typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		}

		public ICommandConfigurator WithGuards<T1, T2, T3, T4, T5>()
		{
			return WithGuards (typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}
		
		/**
		 * @inheritDoc
		 */
		public ICommandConfigurator WithHooks(params object[] hooks)
		{
			_mapping.AddHooks(hooks);
			return this;
		}

		public ICommandConfigurator WithHooks<T>()
		{
			return WithHooks (typeof(T));
		}

		public ICommandConfigurator WithHooks<T1, T2>()
		{
			return WithHooks (typeof(T1), typeof(T2));
		}

		public ICommandConfigurator WithHooks<T1, T2, T3>()
		{
			return WithHooks (typeof(T1), typeof(T2), typeof(T3));
		}

		public ICommandConfigurator WithHooks<T1, T2, T3, T4>()
		{
			return WithHooks (typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		}

		public ICommandConfigurator WithHooks<T1, T2, T3, T4, T5>()
		{
			return WithHooks (typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}
		
		/**
		 * @inheritDoc
		 */
		public ICommandConfigurator WithExecuteMethod(string name)
		{
			_mapping.SetExecuteMethod(name);
			return this;
		}
		
		/**
		 * @inheritDoc
		 */
		public ICommandConfigurator WithPayloadInjection(bool value)
		{
			_mapping.SetPayloadInjectionEnabled(value);
			return this;
		}
	}
}


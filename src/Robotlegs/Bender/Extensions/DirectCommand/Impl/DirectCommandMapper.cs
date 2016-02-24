//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Extensions.CommandCenter.Impl;
using Robotlegs.Bender.Extensions.DirectCommand.API;

namespace Robotlegs.Bender.Extensions.DirectCommand.Impl
{
	public class DirectCommandMapper : IDirectCommandConfigurator
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private ICommandMappingList _mappings;

		private ICommandMapping _mapping;

		private ICommandExecutor _executor;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
	
		public DirectCommandMapper(ICommandExecutor executor, ICommandMappingList mappings, Type commandClass)
		{
			_executor = executor;
			_mappings = mappings;
			_mapping = new CommandMapping (commandClass);
			_mapping.SetFireOnce (true);
			_mappings.AddMapping (_mapping);
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IDirectCommandConfigurator WithExecuteMethod (string name)
		{
			_mapping.SetExecuteMethod (name);
			return this;
		}

		public IDirectCommandConfigurator WithGuards (params object[] guards)
		{
			_mapping.AddGuards(guards);
			return this;
		}

		public IDirectCommandConfigurator WithHooks (params object[] hooks)
		{
			_mapping.AddHooks (hooks);
			return this;
		}

		public IDirectCommandConfigurator WithPayloadInjection (bool value = true)
		{
			_mapping.SetPayloadInjectionEnabled (value);
			return this;
		}

		public void Execute (CommandPayload payload)
		{
			_executor.ExecuteCommands (_mappings.GetList (), payload);
		}

		public IDirectCommandConfigurator Map (Type commandClass)
		{
			return new DirectCommandMapper (_executor, _mappings, commandClass);
		}

		public IDirectCommandConfigurator Map<T>()
		{
			return new DirectCommandMapper (_executor, _mappings, typeof(T));
		}
	}
}


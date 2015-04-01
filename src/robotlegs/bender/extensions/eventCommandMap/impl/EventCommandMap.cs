//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.commandCenter.api;
using robotlegs.bender.extensions.commandCenter.dsl;
using robotlegs.bender.extensions.commandCenter.impl;
using robotlegs.bender.extensions.eventCommandMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.eventCommandMap.impl
{
	public class EventCommandMap : IEventCommandMap
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private List<CommandMappingList.Processor> _mappingProcessors = new List<CommandMappingList.Processor>();

		private IInjector _injector;

		private IEventDispatcher _dispatcher;

		private CommandTriggerMap _triggerMap;

		private ILogger _logger;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public EventCommandMap (IContext context, IEventDispatcher dispatcher)
		{
			_injector = context.injector;
			_logger = context.GetLogger (this);
			_dispatcher = dispatcher;
			_triggerMap = new CommandTriggerMap (GetKey, CreateTrigger);
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public ICommandMapper Map (Enum type, Type eventClass)
		{
			return GetTrigger (type, eventClass).CreateMapper();
		}

		public ICommandMapper Map <T>(Enum type)
		{
			return Map (type, typeof(T));
		}

		public ICommandUnmapper Unmap (Enum type, Type eventClass)
		{
			return GetTrigger (type, eventClass).CreateMapper ();
		}

		public ICommandUnmapper Unmap <T>(Enum type)
		{
			return Unmap (type, typeof(T));
		}

		public IEventCommandMap AddMappingProcessor(CommandMappingList.Processor handler)
		{
			if (!_mappingProcessors.Contains(handler))
				_mappingProcessors.Add(handler);
			return this;
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private object GetKey(params object[] args)
		{
			// TODO: Make this event class and type not return string, but object that can be evaluated as unique for both
			Enum type = args [0] as Enum;
			Type eventClass = args [1] as Type;
			if (eventClass == null)
				return type;
			return type.ToString () + eventClass.Name.ToString ();
		}

		private EventCommandTrigger GetTrigger(Enum type, Type eventClass)
		{
			return _triggerMap.GetTrigger (new object[]{ type, eventClass }) as EventCommandTrigger;
		}

		private ICommandTrigger CreateTrigger(params object[] args)
		{
			// TODO: Rethink this delegate structure. Don't like these two lines!
			Enum type = args [0] as Enum;
			Type eventClass = args [1] as Type;
			return new EventCommandTrigger (_injector, _dispatcher, type, eventClass, _mappingProcessors, _logger);
		}
	}
}


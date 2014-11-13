using System;
using robotlegs.bender.extensions.eventCommandMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.commandCenter.api;
using robotlegs.bender.extensions.commandCenter.dsl;
using System.Collections.Generic;
using robotlegs.bender.extensions.commandCenter.impl;
using strange.extensions.injector.api;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.eventCommandMap.impl
{
	public class EventCommandMap : IEventCommandMap
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private List<CommandMappingList.Processor> _mappingProcessors = new List<CommandMappingList.Processor>();

		private IInjectionBinder _injector;

		private IEventDispatcher _dispatcher;

		private CommandTriggerMap _triggerMap;

		private ILogger _logger;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public EventCommandMap (IContext context, IEventDispatcher dispatcher)
		{
			_injector = context.injectionBinder;
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

		public ICommandUnmapper Unmap (Enum type, Type eventClass)
		{
			return GetTrigger (type, eventClass).CreateMapper ();
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
			// TODO: Rethink this delegate structure. Don't like these two lines! Also enum should be used so this might be redundant
			Enum type = args [0] as Enum;
			Type eventClass = args [1] as Type;
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


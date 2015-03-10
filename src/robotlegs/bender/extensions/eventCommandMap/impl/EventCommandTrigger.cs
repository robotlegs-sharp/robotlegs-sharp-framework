using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.commandCenter.api;
using System.Collections.Generic;
using robotlegs.bender.extensions.commandCenter.dsl;
using robotlegs.bender.extensions.commandCenter.impl;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.eventCommandMap.impl
{
	public class EventCommandTrigger : ICommandTrigger
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IEventDispatcher _dispatcher;

		private Enum _type;

		private Type _eventClass;

		private ICommandMappingList _mappings;

		private ICommandExecutor _executor;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public EventCommandTrigger (IInjector injector, IEventDispatcher dispatcher, Enum type, Type eventClass = null, IEnumerable<CommandMappingList.Processor> processors = null, ILogger logger = null)
		{
			_dispatcher = dispatcher;
			_type = type;
			_eventClass = eventClass;
			_mappings = new CommandMappingList(this, processors, logger);
			_executor = new CommandExecutor(injector, _mappings.RemoveMapping);
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public CommandMapper CreateMapper()
		{
			return new CommandMapper(_mappings);
		}

		public void Activate ()
		{
			_dispatcher.AddEventListener<IEvent> (_type, EventHandler);
		}

		public void Deactivate ()
		{
			_dispatcher.RemoveEventListener<IEvent> (_type, EventHandler);
		}

		public override string ToString ()
		{
			return _eventClass + " with selector '" + _type.ToString() + "'";
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void EventHandler(IEvent evt)
		{
			/**
			 * Map(CustomType, typeof(IEvent)). 	Dispatch(new CustomEvent(CustomType)).	Make it inject IEvent
			 * Map(CustomType). 					Dispatch(new Event(CustomType)).		Make it inject IEvent
			 * Map(CustomType). 					Dispatch(new CustomEvent(CustomType)).	Make it inject CustomEvent
			 * 
			 * Map(CustomType).typeof(CustomType)	Dispatch(new Event(CustomType)).		Make it not execute
			 */

			Type evtType = evt.GetType ();
			Type payloadEvtType = null;
			if (evtType == _eventClass || (_eventClass == null))
			{
				payloadEvtType = (evtType == typeof(Event)) ? typeof(IEvent) : evtType;
			}
			else if (_eventClass == typeof(IEvent))
			{
				payloadEvtType = _eventClass;
				payloadEvtType = typeof (IEvent);
			}
			else
				return;

			_executor.ExecuteCommands(_mappings.GetList(), new CommandPayload (new List<object>{ evt }, new List<Type>{ payloadEvtType }));
		}
	}
}


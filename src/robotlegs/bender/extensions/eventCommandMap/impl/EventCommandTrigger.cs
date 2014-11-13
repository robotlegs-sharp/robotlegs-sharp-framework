using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.commandCenter.api;
using System.Collections.Generic;
using robotlegs.bender.extensions.commandCenter.dsl;
using robotlegs.bender.extensions.commandCenter.impl;
using strange.extensions.injector.api;
using robotlegs.bender.framework.api;

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

		public EventCommandTrigger (IInjectionBinder injector, IEventDispatcher dispatcher, Enum type, Type eventClass = null, List<CommandMappingList.Processor> processors = null, ILogger logger = null)
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
			Console.WriteLine ("Event handler: " + evt.type);
			// TODO: Understand what code should go here? Or why they are event checking and if it's needed
			/*
			const eventConstructor:Class = event["constructor"] as Class;
			var payloadEventClass:Class;
			//not pretty, but optimized to avoid duplicate checks and shortest paths
			if (eventConstructor == _eventClass || (!_eventClass))
			{
				payloadEventClass = eventConstructor;
			}
			else if (_eventClass == Event)
			{
				payloadEventClass = _eventClass;
			}
			else
			{
				return;
			}
			//*/

			_executor.ExecuteCommands(_mappings.GetList(), new CommandPayload (new List<object>{ evt }, new List<Type>{ evt.GetType () }));
		}
	}
}


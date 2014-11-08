using System;
using robotlegs.bender.extensions.eventCommandMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.commandCenter.api;
using robotlegs.bender.extensions.commandCenter.dsl;
using System.Collections.Generic;
using robotlegs.bender.extensions.commandCenter.impl;
using strange.extensions.injector.api;

namespace robotlegs.bender.extensions.eventCommandMap.impl
{
	public class EventCommandMap : IEventCommandMap
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private List<CommandMappingList.Processor> _mappingProcessors;

		private IInjectionBinder _injector;

		private IEventDispatcher _dispatcher;

//		private CommandTriggerMap

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public EventCommandMap ()
		{
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public ICommandMapper map (string type, Type eventClass)
		{
			throw new NotImplementedException ();
		}

		public ICommandUnmapper unmap (string type, Type eventClass)
		{
			throw new NotImplementedException ();
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/
	}
}


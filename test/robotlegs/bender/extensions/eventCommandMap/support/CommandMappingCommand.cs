using System;
using robotlegs.bender.extensions.eventDispatcher.impl;
using robotlegs.bender.extensions.eventCommandMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.eventCommandMap.support
{
	public class CommandMappingCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IEvent evt;

		[Inject("nestedCommand")]
		public Type commandClass;

		[Inject]
		public IEventCommandMap eventCommandMap;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			eventCommandMap.Map(evt.type, typeof(IEvent)).ToCommand(commandClass);
		}
	}
}


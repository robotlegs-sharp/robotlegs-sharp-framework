using System;
using robotlegs.bender.extensions.eventCommandMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.commandCenter.support;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.eventCommandMap.support
{
	public class CascadingCommand
	{
		/*============================================================================*/
		/* Public Static Properties                                                   */
		/*============================================================================*/

		public enum EventType
		{
			CASCADING_EVENT
		}

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IEventDispatcher dispatcher;

		[Inject]
		public IEventCommandMap eventCommandMap;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			eventCommandMap
				.Map(EventType.CASCADING_EVENT)
				.ToCommand<NullCommand>().Once();

			dispatcher.Dispatch(new Event(EventType.CASCADING_EVENT));
		}
	}
}


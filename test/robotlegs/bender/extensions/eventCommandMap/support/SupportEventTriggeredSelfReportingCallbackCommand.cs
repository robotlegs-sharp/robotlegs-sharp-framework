using System;
using robotlegs.bender.extensions.eventDispatcher.impl;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.eventCommandMap.support
{
	public class SupportEventTriggeredSelfReportingCallbackCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject(true)]
		public IEvent untypedEvent;

		[Inject(true)]
		public SupportEvent typedEvent;

		[Inject("ExecuteCallback")]
		public Action<SupportEventTriggeredSelfReportingCallbackCommand> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			callback(this);
		}
	}
}


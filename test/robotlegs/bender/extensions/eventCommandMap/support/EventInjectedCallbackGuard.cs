using System;
using robotlegs.bender.extensions.eventDispatcher.impl;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.eventCommandMap.support
{
	public class EventInjectedCallbackGuard
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IEvent evt;

		[Inject("ApproveCallback")]
		public Action<EventInjectedCallbackGuard> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public bool Approve()
		{
			callback(this);
			return true;
		}
	}
}


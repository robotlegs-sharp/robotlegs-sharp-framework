using System;

namespace robotlegs.bender.extensions.eventCommandMap.support
{
	public class EventParametersCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ExecuteCallback")]
		public Action<SupportEvent> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute(SupportEvent evt)
		{
			callback(evt);
		}
	}
}


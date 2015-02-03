using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class SelfReportingCallbackCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ExecuteCallback")]
		public Action<SelfReportingCallbackCommand> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			callback(this);
		}
	}
}


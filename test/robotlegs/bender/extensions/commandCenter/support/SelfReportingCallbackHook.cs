using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class SelfReportingCallbackHook
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public SelfReportingCallbackCommand command;

		[Inject("HookCallback")]
		public Action<SelfReportingCallbackHook> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Hook()
		{
			callback(this);
		}
	}
}


using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class PayloadInjectionPointsHook
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public string message;

		[Inject]
		public int code;

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Hook()
		{
			reportingFunc(message);
			reportingFunc(code);
		}
	}
}


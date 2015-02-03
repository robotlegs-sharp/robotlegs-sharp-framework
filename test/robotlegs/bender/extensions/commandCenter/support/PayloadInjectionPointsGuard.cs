using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class PayloadInjectionPointsGuard
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

		public bool Approve()
		{
			reportingFunc(message);
			reportingFunc(code);
			return true;
		}
	}
}


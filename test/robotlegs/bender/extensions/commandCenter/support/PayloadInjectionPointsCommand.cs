using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class PayloadInjectionPointsCommand
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

		public void Execute()
		{
			reportingFunc(message);
			reportingFunc(code);
		}
	}
}


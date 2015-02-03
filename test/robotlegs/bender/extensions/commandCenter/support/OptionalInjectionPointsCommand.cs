using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class OptionalInjectionPointsCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		[Inject(true)]
		public string message;

		[Inject(true)]
		public int code;

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


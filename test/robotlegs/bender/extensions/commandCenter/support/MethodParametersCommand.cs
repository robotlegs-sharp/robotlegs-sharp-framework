using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class MethodParametersCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute(string message, int code)
		{
			reportingFunc(message);
			reportingFunc(code);
		}
	}
}


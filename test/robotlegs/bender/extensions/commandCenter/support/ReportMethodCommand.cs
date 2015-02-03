using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class ReportMethodCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Report()
		{
			reportingFunc(typeof(ReportMethodCommand));
		}
	}
}


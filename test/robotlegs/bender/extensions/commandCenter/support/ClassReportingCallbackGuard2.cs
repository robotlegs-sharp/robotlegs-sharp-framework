using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class ClassReportingCallbackGuard2
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public bool Approve()
		{
			if (reportingFunc != null)
			{
				reportingFunc(typeof(ClassReportingCallbackGuard2));
			}
			return true;
		}
	}
}


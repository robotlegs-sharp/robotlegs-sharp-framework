using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class ClassReportingCallbackHook
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Hook()
		{
			if (reportingFunc != null)
			{
				reportingFunc(typeof(ClassReportingCallbackHook));
			}
		}
	}
}


using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class ClassReportingCallbackCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			if (reportingFunc != null) 
			{
				reportingFunc (typeof(ClassReportingCallbackCommand));
			}
		}
	}
}


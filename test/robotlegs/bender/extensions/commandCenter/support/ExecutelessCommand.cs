using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class ExecutelessCommand
	{	
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("ReportingFunction")]
		public Action<object> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		[PostConstruct]
		public void init()
		{
			reportingFunc(typeof(ExecutelessCommand));
		}
	}
}


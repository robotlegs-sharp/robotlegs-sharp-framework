using System;
using robotlegs.bender.extensions.directCommandMap.api;

namespace robotlegs.bender.extensions.directCommandMap.support
{
	public class DirectCommandMapReportingCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IDirectCommandMap dcm;

		[Inject("ReportingFunction")]
		public Action<IDirectCommandMap> reportingFunc;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			reportingFunc(dcm);
		}
	}
}


using System;
using robotlegs.bender.extensions.commandCenter.api;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class AbstractInterfaceImplementingCommand : ICommand
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
			reportingFunc(typeof(AbstractInterfaceImplementingCommand));
		}
	}
}


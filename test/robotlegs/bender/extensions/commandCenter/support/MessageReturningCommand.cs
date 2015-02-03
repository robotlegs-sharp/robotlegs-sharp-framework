using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class MessageReturningCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public string message;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public string Execute()
		{
			return message;
		}
	}
}


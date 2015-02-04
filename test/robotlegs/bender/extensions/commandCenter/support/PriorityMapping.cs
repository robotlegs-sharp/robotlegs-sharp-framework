using System;
using robotlegs.bender.extensions.commandCenter.impl;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class PriorityMapping : CommandMapping
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public int priority;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public PriorityMapping (Type command, int priority) : base(command)
		{
			this.priority = priority;
		}
	}
}


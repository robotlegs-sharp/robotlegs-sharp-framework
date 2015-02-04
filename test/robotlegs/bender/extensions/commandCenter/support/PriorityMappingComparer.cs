using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.commandCenter.api;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class PriorityMappingComparer : IComparer<ICommandMapping>
	{
		public int Compare (ICommandMapping a, ICommandMapping b)
		{
			int aPriority = (a is PriorityMapping) ? (a as PriorityMapping).priority : 0;
			int bPriority = (b is PriorityMapping) ? (b as PriorityMapping).priority : 0;
			if (aPriority == bPriority)
				return 0;
			return aPriority > bPriority ? 1 : -1;
		}
	}
}


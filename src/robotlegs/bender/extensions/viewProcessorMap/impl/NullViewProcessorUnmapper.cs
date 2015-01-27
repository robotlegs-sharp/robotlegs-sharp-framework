using System;
using robotlegs.bender.extensions.viewProcessorMap.dsl;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public class NullViewProcessorUnmapper : IViewProcessorUnmapper
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void FromProcess(object processorClassOrInstance)
		{
		}

		public void FromAll()
		{
		}

		public void FromNoProcess()
		{
		}

		public void FromInjection()
		{
		}
	}
}


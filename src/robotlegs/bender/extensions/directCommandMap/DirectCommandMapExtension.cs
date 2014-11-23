using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.directCommandMap.api;

namespace robotlegs.bender.extensions.directCommandMap
{
	public class DirectCommandMapExtension : IExtension
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.injector.Map(typeof(IDirectCommandMap)).ToType(typeof(DirectCommandMap));
		}
	}
}


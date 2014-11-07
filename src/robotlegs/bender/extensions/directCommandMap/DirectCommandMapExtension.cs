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
			context.injectionBinder.Bind (typeof(IDirectCommandMap)).To (typeof(DirectCommandMap));
		}
	}
}


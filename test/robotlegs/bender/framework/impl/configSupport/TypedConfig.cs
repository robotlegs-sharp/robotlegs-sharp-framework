using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl.configSupport
{
	public class TypedConfig : IConfig
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("callback")]
		public Action<TypedConfig> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Configure ()
		{
			callback (this);
		}
	}
}


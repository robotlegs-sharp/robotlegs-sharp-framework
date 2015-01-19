using System;

namespace robotlegs.bender.framework.impl.configSupport
{
	public class UntypedConfig
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("callback")]
		public Action<UntypedConfig> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Configure()
		{
			callback(this);
		}
	}
}


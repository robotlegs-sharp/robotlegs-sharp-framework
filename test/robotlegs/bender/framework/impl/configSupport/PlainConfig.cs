using System;

namespace robotlegs.bender
{
	public class PlainConfig
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject("callback")]
		public Action<PlainConfig> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		[PostConstruct]
		public void init()
		{
			callback(this);
		}
	}
}


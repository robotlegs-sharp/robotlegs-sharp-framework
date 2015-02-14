using System;

namespace robotlegs.bender.extensions.mediatorMap.support
{

	public class CallbackMediator
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject(true, "callback")]
		public Action<object> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		[PostConstruct]
		public void Init()
		{
			if (callback != null)
			{
				callback(this);
			}
		}
	}
}

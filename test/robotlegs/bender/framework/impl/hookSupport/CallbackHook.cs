using System;

namespace robotlegs.bender.framework.impl.hookSupport
{
	public class CallbackHook
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject(true, "hookCallback")]
		public Action callback;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public CallbackHook(Action callback = null)
		{
			this.callback = callback;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Hook()
		{
			if (callback != null)
				callback();
		}
	}
}


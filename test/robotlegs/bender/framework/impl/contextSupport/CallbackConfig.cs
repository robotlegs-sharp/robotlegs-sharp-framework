using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl.contextSupport
{
	public class CallbackConfig : IConfig
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action _callback;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public CallbackConfig(Action callback)
		{
			_callback = callback;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Configure()
		{
			_callback();
		}
	}
}


using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl.contextSupport
{
	public class CallbackExtension : IExtension
	{
		/*============================================================================*/
		/* Public Static Properties                                                   */
		/*============================================================================*/

		public static Action<IContext> staticCallback;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<IContext> _callback;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public CallbackExtension(Action<IContext> callback = null)
		{
			_callback = callback == null ? staticCallback : callback;
			staticCallback = null;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend(IContext context)
		{
//			_callback && safelyCallBack(_callback, null, context);
			_callback (context);
		}
	}
}


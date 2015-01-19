using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.enhancedLogging.impl;

namespace robotlegs.bender.extensions.enhancedLogging
{
	/// <summary>
	/// This extension logs messages for all Injector actions.
	/// 
	/// Warning: this extension will degrade the performance of your application.
	/// </summary>
	public class InjectorActivityLoggingExtension : IExtension/// 
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			InjectorListener listener = new InjectorListener(
				context.injector, context.GetLogger(this));
//			context.afterDestroying(listener.destroy);
			context.AfterDestroying (listener.Destroy);
		}
	}
}


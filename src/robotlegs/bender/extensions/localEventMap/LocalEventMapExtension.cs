using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.localEventMap.api;
using robotlegs.bender.extensions.localEventMap.impl;

namespace robotlegs.bender.extensions.localEventMap
{
	/// <summary>
	/// An Event Map keeps track of listeners and provides the ability to
	/// unregister all the listeners which a single method call.
	/// </summary>
	public class LocalEventMapExtension : IExtension
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.injector.Map(typeof(IEventMap)).ToType((typeof(EventMap)));
		}
	}
}


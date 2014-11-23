using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.eventCommandMap.api;
using robotlegs.bender.extensions.eventCommandMap.impl;

namespace robotlegs.bender.extensions.eventCommandMap
{
	/// <summary>
	/// The Event Command Map allows you to bind Events to Commands
	/// </summary>

	public class EventCommandMapExtension : IExtension
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.injector.Map(typeof(IEventCommandMap)).ToSingleton(typeof(EventCommandMap));
		}
	}
}


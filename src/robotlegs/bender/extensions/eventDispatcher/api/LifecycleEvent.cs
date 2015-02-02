using System;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.eventDispatcher.api
{
	public class LifecycleEvent : Event
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public enum Type
		{
			ERROR,
			STATE_CHANGE,
			PRE_INITIALIZE,
			INITIALIZE,
			POST_INITIALIZE,
			PRE_SUSPEND,
			SUSPEND,
			POST_SUSPEND,
			PRE_RESUME,
			RESUME,
			POST_RESUME,
			PRE_DESTROY,
			DESTROY,
			POST_DESTROY
		}

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public LifecycleEvent (Type type) : base(type)
		{

		}
	}
}


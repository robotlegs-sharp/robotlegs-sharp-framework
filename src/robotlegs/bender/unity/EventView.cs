using System;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace strange.extensions.mediation.impl
{
	public class EventView : View
	{
		public IEventDispatcher dispatcher{ get; set;}
	}
}


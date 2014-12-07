using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace strange.extensions.mediation.impl
{
	public class EventView : View
	{
		private IEventDispatcher _dispatcher = new EventDispatcher();
		public IEventDispatcher dispatcher
		{
			get
			{
				return _dispatcher;
			}
			set
			{
				_dispatcher = value;
			}
		}
	}
}


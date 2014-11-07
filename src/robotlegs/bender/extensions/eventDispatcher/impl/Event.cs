using System;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.eventDispatcher.impl
{
	public class Event : IEvent
	{
		private Enum _type;

		public Enum type 
		{
			get 
			{
				return _type;
			}
		}

		public Event (Enum type)
		{
			_type = type;
		}
	}
}


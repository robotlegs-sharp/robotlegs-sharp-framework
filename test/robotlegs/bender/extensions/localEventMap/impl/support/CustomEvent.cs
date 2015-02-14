using robotlegs.bender.extensions.eventDispatcher.api;
using System;

namespace robotlegs.bender.extensions.localEventMap.impl.support
{
	public class CustomEvent : IEvent
	{
		public enum Type
		{
			STARTED
		}

		private Enum _type;

		public Enum type 
		{
			get 
			{
				return _type;
			}
		}

		public CustomEvent (CustomEvent.Type type)
		{
			_type = type;
		}

	}
}
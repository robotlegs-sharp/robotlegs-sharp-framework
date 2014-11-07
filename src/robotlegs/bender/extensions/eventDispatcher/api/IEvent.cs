using System;

namespace robotlegs.bender.extensions.eventDispatcher.api
{
	public interface IEvent
	{
		Enum type { get; }
	}
}


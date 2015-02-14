using System;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.localEventMap.api
{
	public interface IEventMap
	{
		void MapListener(IEventDispatcher dispatcher, Enum type, Delegate listener, Type eventClass = null);

		void UnmapListener(IEventDispatcher dispatcher, Enum type, Delegate listener, Type eventClass = null);

		void UnmapListeners();

		void Suspend();

		void Resume();
	}
}


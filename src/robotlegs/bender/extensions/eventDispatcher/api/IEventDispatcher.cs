using System;

namespace robotlegs.bender.extensions.eventDispatcher.api
{
	public interface IEventDispatcher : IDispatcher
	{
		void AddEventListener<T> (Enum type, Action<T> listener);
		void AddEventListener(Enum type, Delegate listener);
		void RemoveEventListener<T>(Enum type, Action<T> listener);
		void RemoveEventListener(Enum type, Delegate listener);
		bool HasEventListener (Enum type);
	}
}


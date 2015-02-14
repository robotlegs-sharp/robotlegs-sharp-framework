using System;

namespace robotlegs.bender.extensions.eventDispatcher.api
{
	public interface IEventDispatcher : IDispatcher
	{
		void AddEventListener<T> (Enum type, Action<T> listener);
		void AddEventListener (Enum type, Action<IEvent> listener);
		void AddEventListener (Enum type, Action listener);
		void AddEventListener(Enum type, Delegate listener);
		void RemoveEventListener<T>(Enum type, Action<T> listener);
		void RemoveEventListener(Enum type, Action<IEvent> listener);
		void RemoveEventListener(Enum type, Action listener);
		void RemoveEventListener(Enum type, Delegate listener);
		void RemoveAllEventListeners();
		bool HasEventListener (Enum type);
	}
}


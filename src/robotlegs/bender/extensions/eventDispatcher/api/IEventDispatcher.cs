using System;

namespace robotlegs.bender.extensions.eventDispatcher.api
{
	public interface IEventDispatcher : IDispatcher
	{
		void AddEventListener<T> (Enum type, Action<T> listener);
		void AddEventListener<T> (Enum type, Delegate listener);
//		void AddEventListener(Enum type, MulticastDelegate listener);
		void RemoveEventListener<T>(Enum type, Action<T> listener);
		void RemoveEventListener<T>(Enum type, Delegate listener);
//		void RemoveEventListener(Enum type, MulticastDelegate listener);
	}
}


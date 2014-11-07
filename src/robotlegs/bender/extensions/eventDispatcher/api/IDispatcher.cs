using System;

namespace robotlegs.bender.extensions.eventDispatcher.api
{
	public interface IDispatcher
	{
		void Dispatch (IEvent evt);
	}
}


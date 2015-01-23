using System;

namespace robotlegs.bender.extensions.viewManager.api
{
	public interface IViewStateWatcher
	{
		bool isAdded { get; }

		event Action<object> added;
		event Action<object> removed;
		event Action<object> disabled;
		event Action<object> enabled;
	}
}


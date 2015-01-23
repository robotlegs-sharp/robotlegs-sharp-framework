using System;

namespace robotlegs.bender.extensions.contextview.api
{
	public interface IContextView
	{
		object view { get; }

		event Action contextViewStateWatcherSet;
		IContextViewStateWatcher contextViewStateWatcher { get; }
	}

	internal interface IContextViewStateWatcherSetter
	{
		IContextViewStateWatcher contextViewStateWatcher { set; }
	}
}


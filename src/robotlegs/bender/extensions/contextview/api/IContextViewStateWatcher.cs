using System;

namespace robotlegs.bender.extensions.contextview.api
{
	public interface IContextViewStateWatcher
	{
		bool isAddedToStage { get; }

		event Action<object> addedToStage;
		event Action<object> removeFromStage;
		event Action<object> suspended;
		event Action<object> resumed;
	}
}


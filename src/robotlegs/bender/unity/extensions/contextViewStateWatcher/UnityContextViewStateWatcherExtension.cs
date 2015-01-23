using robotlegs.bender.extensions.contextview.api;
using UnityEngine;
using robotlegs.bender.extensions.contextview;
using robotlegs.bender.unity.extensions.contextviewstatewatcher;

namespace robotlegs.bender.extensions.contextviewstatewatcher
{
	public class UnityContextViewStateWatcherExtension : ContextViewStateWatcherExtension
	{

		protected override IContextViewStateWatcher GetContextViewStateWatcher (object contextView)
		{
			Component castContextView = contextView as Component;
			if(castContextView == null)
			{
				_logger.Warn("ContextView is not a Component. {0}", contextView);
				return null;
			}
			return castContextView.gameObject.AddComponent<UnityContextViewStateWatcher>();
		}

	}
}


using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewManager.api;
using UnityEngine;
using robotlegs.bender.platforms.unity.extensions.viewManager.impl;

namespace robotlegs.bender.platforms.unity.extensions.viewManager.impl
{
	public class UnityViewStateWatcherExtension : ViewStateWatcherExtension
	{
		protected override IViewStateWatcher GetViewStateWatcher (object contextView)
		{
			GameObject target = null;
			if(contextView is GameObject) target = contextView as GameObject;
			else if(contextView is Component) target = (contextView as Component).gameObject;

			if(target == null)
			{
				_logger.Warn("ContextView is not a Component or a GameObject, {0}", contextView);
				return null;
			}
			return target.AddComponent<UnityViewStateWatcher>();
		}
	}
}
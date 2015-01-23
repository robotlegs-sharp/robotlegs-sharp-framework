using UnityEngine;
using robotlegs.bender.unity.extensions.viewManager.impl;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.contextview;

namespace robotlegs.bender.unity.extensions.contextview
{
	public class UnityStageSyncExtension : StageSyncExtension
	{
		protected override IViewStateWatcher GetContextViewStateWatcher (object contextView)
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
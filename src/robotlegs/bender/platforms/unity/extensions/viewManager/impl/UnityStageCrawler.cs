using System;
using robotlegs.bender.extensions.viewManager.impl;
using UnityEngine;
using robotlegs.bender.extensions.mediatorMap.api;

namespace robotlegs.bender.platforms.unity.extensions.viewManager.impl
{
	public class UnityStageCrawler : StageCrawler
	{
		protected override void ScanContainer (object container)
		{
			Transform containerTransform = null;
			if (container is GameObject)
			{
				containerTransform = (container as GameObject).transform;
			}
			else if (container is Transform)
			{
				containerTransform = container as Transform;
			}
			else
				return;

			ProcessViewsFromRoot (containerTransform);
		}

		private void ProcessViewsFromRoot(Transform view)
		{
			MonoBehaviour[] viewScripts = view.GetComponentsInChildren<MonoBehaviour>(false);

			foreach (MonoBehaviour viewScript in viewScripts)
			{
				if (viewScript is IView)
				{
					ProcessView (viewScript);
				}
			}
		}
	}
}


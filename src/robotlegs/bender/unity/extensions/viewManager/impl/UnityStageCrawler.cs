using System;
using robotlegs.bender.extensions.viewManager.impl;
using UnityEngine;

namespace robotlegs.bender.unity.extensions.viewManager.impl
{
	public class UnityStageCrawler : StageCrawler
	{
		protected override void ScanContainer (object container)
		{
			Transform containerTransform = null;
			if (container is GameObject)
			{
				GameObject containerGameObject = container as GameObject;
				ProcessViewMonobehaviours(containerGameObject);
				containerTransform = containerGameObject.transform;
			}
			else if (container is Transform)
			{
				containerTransform = container as Transform;
				ProcessViewMonobehaviours(containerTransform.gameObject);
			}
			if (containerTransform == null)
				return;

			int numChildren = containerTransform.childCount;
			for (int i = 0; i < numChildren; i++)
			{
				Transform child = containerTransform.GetChild(i);
				if(child.childCount != 0)
				{
					ScanContainer (child.gameObject);
				}
				else
				{
					ProcessViewMonobehaviours(child.gameObject);
				}
			}
		}

		private void ProcessViewMonobehaviours(GameObject view)
		{
			MonoBehaviour[] viewScripts = view.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour viewScript in viewScripts)
			{
				ProcessView (viewScript);
			}
		}

	}
}


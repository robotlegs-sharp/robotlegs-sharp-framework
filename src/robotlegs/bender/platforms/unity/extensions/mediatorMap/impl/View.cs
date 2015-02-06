using UnityEngine;
using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.platforms.unity.extensions.mediatorMap.impl
{
	public class View : MonoBehaviour, IView
	{
		public event Action<IView> RemoveView;

		protected virtual void Start ()
		{
			ViewNotifier.RegisterView(this, this.GetType());
		}

		protected virtual void OnDestroy ()
		{
			if (RemoveView != null)
				RemoveView(this);
		}
	}
}


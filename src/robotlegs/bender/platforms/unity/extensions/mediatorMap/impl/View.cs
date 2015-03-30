using UnityEngine;
using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.platforms.unity.extensions.mediatorMap.impl
{
	public class View : MonoBehaviour, IView
	{
		public event Action<IView> RemoveView
		{
			add
			{
				_removeView += value;
			}
			remove
			{
				_removeView -= value;
			}
		}
		
		private Action<IView> _removeView;
		
		protected virtual void Start ()
		{
			ViewNotifier.RegisterView(this);
		}

		protected virtual void OnDestroy ()
		{
			if (_removeView != null)
				_removeView(this);
		}
	}
}


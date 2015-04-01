//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.viewManager.api;
using UnityEngine;

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


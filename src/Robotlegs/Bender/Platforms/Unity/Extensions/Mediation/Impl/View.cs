//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using UnityEngine;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.Mediation.Impl
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

        protected virtual void Start()
        {
            ViewNotifier.RegisterView(this);
        }

        public virtual void Ready()
        {

        }

		protected virtual void OnDestroy ()
		{
			if (_removeView != null)
				_removeView(this);
		}
	}
}


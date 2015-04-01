//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.platforms.unity.extensions.viewManager.impl;
using UnityEngine;

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
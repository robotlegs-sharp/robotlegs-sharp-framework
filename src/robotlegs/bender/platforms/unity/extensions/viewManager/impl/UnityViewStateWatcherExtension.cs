//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ViewManagement;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Platforms.Unity.Extensions.ViewManager.Impl;
using UnityEngine;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.ViewManager.Impl
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
			UnityViewStateWatcher viewStateWatcher = target.AddComponent<UnityViewStateWatcher>();
			viewStateWatcher.target = contextView;
			return viewStateWatcher;
		}
	}
}
//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.mediatorMap.dsl;
using robotlegs.bender.platforms.unity.extensions.unityMediatorManager.impl;

namespace robotlegs.bender.platforms.unity.extensions.unityMediatorManager
{
	public class UnityMediatorManagerExtension : IExtension
	{
		public void Extend (IContext context)
		{
			context.injector.Map (typeof(IMediatorManager)).ToSingleton (typeof(UnityMediatorManager));
		}
	}
}

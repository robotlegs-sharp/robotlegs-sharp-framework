//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using Robotlegs.Bender.Extensions.Mediation.DSL;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Platforms.Unity.Extensions.Mediation.Impl;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.Mediation
{
	public class UnityMediatorManagerExtension : IExtension
	{
		public void Extend (IContext context)
		{
			context.injector.Map (typeof(IMediatorManager)).ToSingleton (typeof(UnityMediatorManager));
		}
	}
}

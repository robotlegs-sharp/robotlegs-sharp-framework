//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ViewManagement;
using Robotlegs.Bender.Extensions.ViewManagement.Impl;
using Robotlegs.Bender.Framework.API;
using robotlegs.bender.framework.unity.extensions.viewManager.impl;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.ViewManager
{
	public class UnityParentFinderExtension : IExtension
	{
		private IInjector _injector;

		private IParentFinder _parentFinder;

		public void Extend (IContext context)
		{
			_injector = context.injector;

			_parentFinder = new UnityParentFinder();
			_injector.Map(typeof(IParentFinder)).ToValue(_parentFinder);
			context.BeforeInitializing(BeforeInitializing);
		}

		private void BeforeInitializing()
		{
			ContainerRegistry registry = _injector.GetInstance(typeof(ContainerRegistry)) as ContainerRegistry;
			registry.SetParentFinder(_parentFinder);
		}
	}
}
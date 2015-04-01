//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewManager.impl;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.unity.extensions.viewManager.impl;

namespace robotlegs.bender.platforms.unity.extensions.viewManager
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
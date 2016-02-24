//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Extensions.ViewManagement.Impl;

namespace Robotlegs.Bender.Extensions.ViewManagement.Support
{
	public class SupportParentFinderExtension : IExtension
	{
		private IInjector _injector;

		private IParentFinder _parentFinder;

		public void Extend (IContext context)
		{
			_injector = context.injector;

			_parentFinder = new SupportParentFinder();
			_injector.Map(typeof(IParentFinder)).ToValue(_parentFinder);
			context.BeforeInitializing(BeforeInitializing);
		}

		private void BeforeInitializing()
		{
			if (_injector.HasDirectMapping (typeof(ContainerRegistry)))
			{
				ContainerRegistry registry = _injector.GetInstance (typeof(ContainerRegistry)) as ContainerRegistry;
				registry.SetParentFinder(_parentFinder);
				_injector.Unmap (typeof(IParentFinder));
				_injector.Map(typeof(IParentFinder)).ToValue(registry);
			}
		}
	}
}


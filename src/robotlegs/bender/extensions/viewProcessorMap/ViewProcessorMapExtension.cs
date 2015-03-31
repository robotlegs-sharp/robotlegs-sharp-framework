//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.viewProcessorMap.api;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.viewProcessorMap.impl;

namespace robotlegs.bender.extensions.viewProcessorMap
{
	public class ViewProcessorMapExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;

		private IViewProcessorMap _viewProcessorMap;

		private IViewManager _viewManager;

		private IViewProcessorFactory _viewProcessorFactory;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend(IContext context)
		{
			context.BeforeInitializing(BeforeInitializing);
			context.BeforeDestroying(BeforeDestroying);
			context.WhenDestroying(WhenDestroying);
			_injector = context.injector;
			_injector.Map(typeof(IViewProcessorFactory)).ToValue(new ViewProcessorFactory(_injector.CreateChild()));
			_injector.Map(typeof(IViewProcessorMap)).ToSingleton(typeof(ViewProcessorMap));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void BeforeInitializing()
		{
			_viewProcessorMap = _injector.GetInstance(typeof(IViewProcessorMap)) as IViewProcessorMap;
			_viewProcessorFactory = _injector.GetInstance(typeof(IViewProcessorFactory)) as IViewProcessorFactory;
			if (_injector.SatisfiesDirectly(typeof(IViewManager)))
			{
				_viewManager = _injector.GetInstance(typeof(IViewManager)) as IViewManager;
				_viewManager.AddViewHandler(_viewProcessorMap as IViewHandler);
			}
		}

		private void BeforeDestroying()
		{
			_viewProcessorFactory.RunAllUnprocessors();

			if (_injector.SatisfiesDirectly(typeof(IViewManager)))
			{
				_viewManager = _injector.GetInstance(typeof(IViewManager)) as IViewManager;
				_viewManager.RemoveViewHandler(_viewProcessorMap as IViewHandler);
			}
		}

		private void WhenDestroying()
		{
			if (_injector.SatisfiesDirectly(typeof(IViewProcessorMap)))
			{
				_injector.Unmap(typeof(IViewProcessorMap));
			}
			if (_injector.SatisfiesDirectly(typeof(IViewProcessorFactory)))
			{
				_injector.Unmap(typeof(IViewProcessorFactory));
			}
		}

	}
}


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.contextview.api;
using UnityEngine;

namespace robotlegs.bender.platforms.unity.extensions.contextview
{
	public class ContextViewTransformExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private IInjector _injector;
		
		private ILogger _logger;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			_injector = context.injector;
			_logger = context.GetLogger(this);
			context.AddConfigHandler(new InstanceOfMatcher (typeof(IContextView)), AddContextView);
		}
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void AddContextView(object contextViewObject)
		{
			IContextView contextView = contextViewObject as IContextView;

			if (!(contextView.view is Transform)) 
			{
				_logger.Warn ("Cannot map {0} as Transform for the ContextViewTransformExtension to work. Try to configure with 'new TransformContextView(transform)'", contextView.view);
				return;
			}

			if (_injector.HasDirectMapping (typeof(Transform))) 
			{
				_logger.Warn ("A Transform has already been mapped, ignoring {0}", contextView.view);
				return;
			}

			_logger.Debug("Mapping {0} as Transform", contextView.view);
			_injector.Map(typeof(Transform)).ToValue(contextView.view);
		}
	}
}


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Framework.API;
using swiftsuspenders.dependencyproviders;

namespace Robotlegs.Bender.Extensions.EnhancedLogging.Impl
{
	public class LoggerProvider : DependencyProvider
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<DependencyProvider, object> PostApply
		{
			add
			{
				_postApply += value;
			}
			remove 
			{
				_postApply -= value;
			}
		}

		public event Action<DependencyProvider, object> PreDestroy
		{
			add
			{
				_preDestroy += value;
			}
			remove 
			{
				_preDestroy -= value;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<DependencyProvider, object> _postApply;

		private Action<DependencyProvider, object> _preDestroy;

		private IContext _context;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public LoggerProvider(IContext context)
		{
			_context = context;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public object Apply (Type targetType, swiftsuspenders.Injector activeInjector, System.Collections.Generic.Dictionary<string, object> injectParameters)
		{
			object logger = _context.GetLogger(targetType);;
			if (_postApply != null)
			{
				_postApply(this, logger);
			}
			return logger;
		}

		public void Destroy ()
		{
			if (_preDestroy != null) 
			{
				_preDestroy(this, null);
			}
		}
	}
}


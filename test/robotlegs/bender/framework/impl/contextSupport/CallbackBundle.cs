//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl.contextSupport
{
	public class CallbackBundle : IBundle
	{
		/*============================================================================*/
		/* Public Static Properties                                                   */
		/*============================================================================*/

		public static Action<IContext> staticCallback;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<IContext> _callback;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public CallbackBundle(Action<IContext> callback = null)
		{
			_callback = callback == null ? staticCallback : callback;
			staticCallback = null;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public virtual void Extend(IContext context)
		{
			_callback (context);
		}
	}
}


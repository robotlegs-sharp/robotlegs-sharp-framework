//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class CallbackViewHandler : IViewHandler
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<object, Type> _callback;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public CallbackViewHandler(Action<object, Type> callback = null)
		{
			_callback = callback;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void HandleView(object view, Type type)
		{
			if (_callback != null)
				_callback(view, type);
		}
	}
}


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
	public class CallbackExtensionInjectable : IExtension
	{
		/*============================================================================*/
		/* Public Static Properties                                                   */
		/*============================================================================*/

		public static Action<CallbackExtensionInjectable> staticCallback;

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IInjector injector;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<CallbackExtensionInjectable> _callback;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public CallbackExtensionInjectable(Action<CallbackExtensionInjectable> callback = null)
		{
			_callback = callback == null ? staticCallback : callback;
			staticCallback = null;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public virtual void Extend(IContext context)
		{
			if (_callback != null)
				_callback (this);
		}
	}
}


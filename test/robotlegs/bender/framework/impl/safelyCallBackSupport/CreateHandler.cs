//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Threading.Tasks;
using System.Threading;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl.safelyCallBackSupport
{
	public class CreateHandler
	{
		public CreateHandler ()
		{
		}

		public static Action Handler(Delegate closure = null, object[] args = null)
		{
			return delegate() {
				if (closure != null)
					closure.DynamicInvoke (args);
			};
		}

		public static Action<T> Handler<T>(Delegate closure = null, object[] args = null)
		{
			return delegate(T arg) {
				if (closure != null)
					closure.DynamicInvoke (args);
			};
		}

		private static int value = 0;

		public static HandlerMessageCallbackDelegate AsyncHandler(Delegate closure = null, object[] args = null)
		{
			return delegate(object message, HandlerAsyncCallback callback) {
				int test = value;
				value++;
				Timer t = new Timer (new TimerCallback (delegate (object state) {
					if (closure != null)
					{
						closure.DynamicInvoke (args);
					}
					callback();
				}), null, 5, System.Threading.Timeout.Infinite);
			};
		}

		public static HandlerMessageCallbackDelegate HandlerThatErrors(Delegate closure = null, object[] args = null)
		{
			return delegate(object message, HandlerAsyncCallback callback) {
				if (closure != null)
					closure.DynamicInvoke (args);
				callback(new Exception("Boom - createCallbackHandlerThatErrors"));
			};
		}
	}
}


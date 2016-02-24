//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

namespace Robotlegs.Bender.Framework.API
{
	public delegate void HandlerMessageDelegate(object message);
	public delegate void HandlerMessageCallbackDelegate(object message, HandlerAsyncCallback callback);
	public delegate void HandlerAsyncCallback(object error = null);

	public delegate void CallbackErrorDelegate(object error);
	public delegate void CallbackErrorMessageDelegate(object error, object message);
}
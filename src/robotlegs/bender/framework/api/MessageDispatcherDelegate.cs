using System;

namespace robotlegs.bender.framework.api
{
	public delegate void HandlerMessageDelegate(object message);
	public delegate void HandlerMessageCallbackDelegate(object message, HandlerAsyncCallback callback);
	public delegate void HandlerAsyncCallback(object error = null);

	public delegate void CallbackErrorDelegate(object error);
	public delegate void CallbackErrorMessageDelegate(object error, object message);
}
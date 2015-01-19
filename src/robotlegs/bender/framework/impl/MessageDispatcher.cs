using System;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
{
	public class MessageDispatcher
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/
		public delegate void HandlerMessageDelegate(object message);
		public delegate void HandlerMessageCallbackDelegate(object message, HandlerAsyncCallback callback);
		public delegate void HandlerAsyncCallback(object error = null);

		public delegate void CallbackErrorDelegate(object error);
		public delegate void CallbackErrorMessageDelegate(object error, object message);

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private static Dictionary<object, List<Delegate>> _handlers = new Dictionary<object, List<Delegate>>();

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public MessageDispatcher ()	
		{
		}

		/**
		 * Registers a message handler with a MessageDispatcher.
		 * @param message The interesting message
		 * @param handler The handler function
		 */
		public void AddMessageHandler(object message, Action handler)
		{
			AddMessageHandler(message, handler as Delegate);
		}

		public void AddMessageHandler(object message, HandlerMessageDelegate handler)
		{
			AddMessageHandler(message, handler as Delegate);
		}

		public void AddMessageHandler(object message, HandlerMessageCallbackDelegate handler)
		{
			AddMessageHandler(message, handler as Delegate);
		}

		private void AddMessageHandler(object message, Delegate handler)
		{
			if (_handlers.ContainsKey(message))
			{
				List<Delegate> messageHandlers = _handlers[message];
				if (!messageHandlers.Contains(handler))
					messageHandlers.Add(handler);
			}
			else
			{
				_handlers[message] = new List<Delegate>{handler};
			}
		}

		/**
		 * Checks whether the MessageDispatcher has any handlers registered for a specific message.
		 * @param message The interesting message
		 * @return A value of true if a handler of the specified message is registered; false otherwise.
		 */
		public bool HasMessageHandler(object message)
		{
			return _handlers.ContainsKey(message);
		}

		/**
		 * Removes a message handler from a MessageDispatcher
		 * @param message The interesting message
		 * @param handler The handler function
		 */
		public void RemoveMessageHandler(object message, Action handler)
		{
			RemoveMessageHandler(message, handler as Delegate);
		}

		public void RemoveMessageHandler(object message, HandlerMessageDelegate handler)
		{
			RemoveMessageHandler(message, handler as Delegate);
		}

		public void RemoveMessageHandler(object message, HandlerMessageCallbackDelegate handler)
		{
			RemoveMessageHandler(message, handler as Delegate);
		}

		private void RemoveMessageHandler(object message, Delegate handler)
		{
			if (_handlers.ContainsKey(message))
			{
				List<Delegate> messageHandlers = _handlers[message];
				messageHandlers.Remove(handler);
				if (messageHandlers.Count == 0)
					_handlers.Remove(message);
			}
		}

		/**
		 * Dispatches a message into the message flow.
		 * @param message The interesting message
		 * @param callback The completion callback function
		 * @param reverse Should handlers be called in reverse order
		 */

		public void DispatchMessage(object message, bool reverse = false)
		{
			DispatchMessage(message, null as Delegate, reverse);
		}

		public void DispatchMessage(object message, Action callback, bool reverse = false)
		{
			DispatchMessage(message, callback as Delegate, reverse);
		}

		public void DispatchMessage(object message, CallbackErrorDelegate callback, bool reverse = false)
		{
			DispatchMessage(message, callback as Delegate, reverse);
		}

		public void DispatchMessage(object message, CallbackErrorMessageDelegate callback, bool reverse = false)
		{
			DispatchMessage(message, callback as Delegate, reverse);
		}

		private void DispatchMessage(object message, Delegate callback = null, bool reverse = false)
		{
			if (_handlers.ContainsKey(message))
			{
				List<Delegate> handlers = new List<Delegate>(_handlers[message]);
				if (reverse)
					handlers.Reverse();
				new MessageRunner(message, handlers, callback).Run();
			}
			else
			{
				InvokeCallback(callback);
			}
		}

		internal static void InvokeCallback(Delegate callbackDelegate, object error = null, object message = null)
		{
			if (callbackDelegate == null)
				return;
			if (callbackDelegate is Action)
				(callbackDelegate as Action)();
			else if (callbackDelegate is CallbackErrorDelegate)
				(callbackDelegate as CallbackErrorDelegate)(error);
			else if (callbackDelegate is CallbackErrorDelegate)
				(callbackDelegate as CallbackErrorMessageDelegate)(error, message);
			else
				throw new Exception("Bad callback signature");
		}
	}

	class MessageRunner
	{
		private object _message;

		private List<Delegate> _handlers = new List<Delegate>();

		private Delegate _callback;

		public MessageRunner(object message, List<Delegate> handlers, Delegate callback)
		{
			_message = message;
			_handlers = handlers;
			_callback = callback;
		}

		public void Run()
		{
			Next ();
		}

		public void Next()
		{
			Delegate handler = null;
			// Try to keep things synchronous with a simple loop,
			// forcefully breaking out for async handlers and recursing.
			// We do this to avoid increasing the stack depth unnecessarily.
			while (_handlers.Count > 0)
			{
				handler = _handlers[0];
				_handlers.RemoveAt(0);

				if (handler is Action)
				{
					(handler as Action)();
				}
				else if (handler is MessageDispatcher.HandlerMessageDelegate) // sync handler: (message)
				{
					(handler as MessageDispatcher.HandlerMessageDelegate)(_message);
				}
				else if (handler is MessageDispatcher.HandlerMessageCallbackDelegate) // sync or async handler: (message, callback)
				{
					bool handled = false;
					(handler as MessageDispatcher.HandlerMessageCallbackDelegate)(_message, delegate(object error)
						{
							// handler must not invoke the callback more than once
							if (handled) return;

							handled = true;
							if (error != null || _handlers.Count == 0)
							{
								MessageDispatcher.InvokeCallback(_callback, error, _message);
							}
							else
							{
								Next();
							}
						});
					// IMPORTANT: MUST break this loop with a RETURN. See top.
					return;
				}
				else // ERROR: this should NEVER happen
				{
					throw new Exception("Bad handler signature");
				}
			}
			// If we got here then this loop finished synchronously.
			// Nobody broke out, so we are done.
			// This relies on the various return statements above. Be careful.
			MessageDispatcher.InvokeCallback(_callback, null, _message);
		}
	}
}



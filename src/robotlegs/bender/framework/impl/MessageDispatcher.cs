using System;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
{
	public class MessageDispatcher
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

//		private const _handlers:Dictionary = new Dictionary();

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
//			const messageHandlers:Array = _handlers[message];
//			if (messageHandlers)
//			{
//				if (messageHandlers.indexOf(handler) == -1)
//					messageHandlers.push(handler);
//			}
//			else
//			{
//				_handlers[message] = [handler];
//			}
		}

		/**
		 * Checks whether the MessageDispatcher has any handlers registered for a specific message.
		 * @param message The interesting message
		 * @return A value of true if a handler of the specified message is registered; false otherwise.
		 */
//		public function hasMessageHandler(message:Object):Boolean
//		{
//			return _handlers[message];
//		}

		/**
		 * Removes a message handler from a MessageDispatcher
		 * @param message The interesting message
		 * @param handler The handler function
		 */
//		public function removeMessageHandler(message:Object, handler:Function):void
//		{
//			const messageHandlers:Array = _handlers[message];
//			const index:int = messageHandlers ? messageHandlers.indexOf(handler) : -1;
//			if (index != -1)
//			{
//				messageHandlers.splice(index, 1);
//				if (messageHandlers.length == 0)
//					delete _handlers[message];
//			}
//		}

		/**
		 * Dispatches a message into the message flow.
		 * @param message The interesting message
		 * @param callback The completion callback function
		 * @param reverse Should handlers be called in reverse order
		 */
//		public function dispatchMessage(message:Object, callback:Function = null, reverse:Boolean = false):void
//		{
//			var handlers:Array = _handlers[message];
//			if (handlers)
//			{
//				handlers = handlers.concat();
//				reverse || handlers.reverse();
//				new MessageRunner(message, handlers, callback).run();
//			}
//			else
//			{
//				callback && safelyCallBack(callback);
//			}
//		}
	}

	class MessageRunner
	{
		private object _message;

		private List<Action> _handlers = new List<Action>();

		private Action _callback;

		public MessageRunner(object message, List<Action> handlers, Action callback)
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
			Action handler = null;
//			while (handler = _handlers[
		}
	}
}



using System;
using NUnit.Framework;
using System.Threading.Tasks;
using robotlegs.bender.framework.impl.safelyCallBackSupport;

namespace robotlegs.bender.framework.impl
{
	[TestFixture]
	public class MessageDispatcherTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private MessageDispatcher dispatcher;

		private object message;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			dispatcher = new MessageDispatcher();
			message = new object();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void addMessageHandler_runs()
		{
			dispatcher.AddMessageHandler(message, delegate() {});
		}

		[Test]
		public void addMessageHandler_stores_handler()
		{
			dispatcher.AddMessageHandler(message, delegate() {});
			Assert.True(dispatcher.HasMessageHandler(message));
		}

		[Test]
		public void hasMessageHandler_runs()
		{
			dispatcher.HasMessageHandler(message);
		}

		[Test]
		public void hasMessageHandler_returns_false()
		{
			Assert.False(dispatcher.HasMessageHandler(message));
		}

		[Test]
		public void hasMessageHandler_returns_true()
		{
			dispatcher.AddMessageHandler(message, delegate() {});
			Assert.True(dispatcher.HasMessageHandler(message));
		}

		[Test]
		public void hasMessageHandler_returns_false_for_wrong_message()
		{
			dispatcher.AddMessageHandler("abcde", delegate() {});
			Assert.False(dispatcher.HasMessageHandler(message));
		}

		[Test]
		public void removeMessageHandler_runs()
		{
			dispatcher.RemoveMessageHandler(message, delegate() {});
		}

		[Test]
		public void removeMessageHandler_removes_the_handler()
		{
			Action handler = delegate() {};
			dispatcher.AddMessageHandler(message, handler);
			dispatcher.RemoveMessageHandler(message, handler);
			Assert.False(dispatcher.HasMessageHandler(message));
		}

		[Test]
		public void removeMessageHandler_does_not_remove_the_wrong_handler()
		{
			Action handler = delegate() {};
			Action otherHandler = delegate() {};
			dispatcher.AddMessageHandler(message, handler);
			dispatcher.AddMessageHandler(message, otherHandler);
			dispatcher.RemoveMessageHandler(message, otherHandler);
			Assert.True(dispatcher.HasMessageHandler(message));
		}

		[Test]
		public void dispatchMessage_runs()
		{
			dispatcher.DispatchMessage(message);
		}

		[Test]
		public void deaf_handler_handles_message()
		{
			bool handled = false;
			dispatcher.AddMessageHandler(message, delegate() {
				handled = true;
			});
			dispatcher.DispatchMessage(message);
			Assert.True(handled);
		}

		[Test]
		public void handler_handles_message()
		{
			object actualMessage = null;
			dispatcher.AddMessageHandler(message, delegate(object msg) {
				actualMessage = msg;
			});
			dispatcher.DispatchMessage(message);
			Assert.AreEqual(actualMessage, message);
		}

		[Test]
		public void message_is_handled_by_multiple_handlers()
		{
			int handleCount = 0;
			dispatcher.AddMessageHandler(message, delegate() {
				handleCount++;
			});
			dispatcher.AddMessageHandler(message, delegate() {
				handleCount++;
			});
			dispatcher.AddMessageHandler(message, delegate() {
				handleCount++;
			});
			dispatcher.DispatchMessage(message);
			Assert.AreEqual(handleCount, 3);
		}

		[Test]
		public void message_is_handled_by_handler_multiple_times()
		{
			int handleCount = 0;
			dispatcher.AddMessageHandler(message, delegate() {
				handleCount++;
			});
			dispatcher.DispatchMessage(message);
			dispatcher.DispatchMessage(message);
			dispatcher.DispatchMessage(message);
			Assert.AreEqual(handleCount, 3);
		}

		[Test]
		public void handler_does_not_handle_the_wrong_message()
		{
			bool handled = false;
			dispatcher.AddMessageHandler(message, delegate() {
				handled = true;
			});
			dispatcher.DispatchMessage("abcde");
			Assert.False(handled);
		}

		[Test]
		public void handler_with_callback_handles_message()
		{
			object actualMessage = null;
			dispatcher.AddMessageHandler(message, delegate(object msg, MessageDispatcher.HandlerAsyncCallback callback) {
				actualMessage = msg;
				callback();
			});
			dispatcher.DispatchMessage(message);
			Assert.AreEqual(actualMessage, message);
		}

		[Test]
		public async Task async_handler_handles_message()
		{
			object actualMessage = null;
			Task awaitTask = null;
			dispatcher.AddMessageHandler(message, delegate(object msg, MessageDispatcher.HandlerAsyncCallback callback) {
				actualMessage = msg;
//				Task.Run(SetTimeout(5, callback as Delegate));
				SetTimeout(5, callback as Delegate).Start();
//				awaitTask = SetTimeout(5, callback as Delegate);
			});
			dispatcher.DispatchMessage(message);

//			await awaitTask;
//			Assert.AreEqual (actualMessage, message);

			await DelayAssertion ((Action)delegate() {
				Assert.AreEqual (actualMessage, message);
			});
		}

		[Test]
		public void callback_is_called_once()
		{
			int callbackCount = 0;
			dispatcher.DispatchMessage(message, delegate() {
				callbackCount++;
			});
			Assert.AreEqual(callbackCount, 1);
		}

		[Test]
		public async Task callback_is_called_once_after_sync_handler()
		{
			int callbackCount = 0;
			dispatcher.AddMessageHandler(message, CreateHandler.Handler());
			dispatcher.DispatchMessage(message, delegate() {
				callbackCount++;
			});
			await DelayAssertion ((Action)delegate() {
				Assert.AreEqual (callbackCount, 1);
			});
		}

		/*
		[Test(async)]
		public function callback_is_called_once_after_async_handler():void
		{
			var callbackCount:int = 0;
			dispatcher.addMessageHandler(message, createAsyncHandler());
			dispatcher.dispatchMessage(message, function():void {
				callbackCount++;
			});
			delayAssertion(function():void {
				assertThat(callbackCount, equalTo(1));
			});
		}

		[Test(async)]
		public function callback_is_called_once_after_sync_and_async_handlers():void
		{
			var callbackCount:int = 0;
			dispatcher.addMessageHandler(message, createAsyncHandler());
			dispatcher.addMessageHandler(message, createHandler());
			dispatcher.addMessageHandler(message, createAsyncHandler());
			dispatcher.addMessageHandler(message, createHandler());
			dispatcher.dispatchMessage(message, function():void {
				callbackCount++;
			});
			delayAssertion(function():void {
				assertThat(callbackCount, equalTo(1));
			}, 100);
		}

		[Test]
		public function handler_passes_error_to_callback():void
		{
			const expectedError:Object = "Error";
			var actualError:Object = null;
			dispatcher.addMessageHandler(message, function(msg:Object, callback:Function):void {
				callback(expectedError);
			});
			dispatcher.dispatchMessage(message, function(error:Object):void {
				actualError = error;
			});
			assertThat(actualError, equalTo(expectedError));
		}

		[Test(async)]
		public function async_handler_passes_error_to_callback():void
		{
			const expectedError:Object = "Error";
			var actualError:Object = null;
			dispatcher.addMessageHandler(message, function(msg:Object, callback:Function):void {
				setTimeout(callback, 5, expectedError);
			});
			dispatcher.dispatchMessage(message, function(error:Object):void {
				actualError = error;
			});
			delayAssertion(function():void {
				assertThat(actualError, equalTo(expectedError));
			});
		}

		[Test]
		public function handler_that_calls_back_more_than_once_is_ignored():void
		{
			var callbackCount:int = 0;
			dispatcher.addMessageHandler(message, function(msg:Object, callback:Function):void {
				callback();
				callback();
			});
			dispatcher.dispatchMessage(message, function(error:Object):void {
				callbackCount++
			});
			assertThat(callbackCount, equalTo(1));
		}

		[Test(async)]
		public function async_handler_that_calls_back_more_than_once_is_ignored():void
		{
			var callbackCount:int = 0;
			dispatcher.addMessageHandler(message, function(msg:Object, callback:Function):void {
				callback();
				callback();
			});
			dispatcher.dispatchMessage(message, function(error:Object):void {
				callbackCount++
			});
			delayAssertion(function():void {
				assertThat(callbackCount, equalTo(1));
			});
		}

		[Test]
		public function sync_handlers_should_run_in_order():void
		{
			const results:Array = [];
			for each (var id:String in['A', 'B', 'C', 'D'])
				dispatcher.addMessageHandler(message, createHandler(results.push, id));
			dispatcher.dispatchMessage(message);
			assertThat(results, array(['A', 'B', 'C', 'D']));
		}

		[Test]
		public function sync_handlers_should_run_in_reverse_order():void
		{
			const results:Array = [];
			for each (var id:String in['A', 'B', 'C', 'D'])
				dispatcher.addMessageHandler(message, createHandler(results.push, id));
			dispatcher.dispatchMessage(message, null, true);
			assertThat(results, array(['D', 'C', 'B', 'A']));
		}

		[Test(async)]
		public function async_handlers_should_run_in_order():void
		{
			const results:Array = [];
			for each (var id:String in['A', 'B', 'C', 'D'])
				dispatcher.addMessageHandler(message, createAsyncHandler(results.push, id));
			dispatcher.dispatchMessage(message);
			delayAssertion(function():void {
				assertThat(results, array(['A', 'B', 'C', 'D']));
			}, 200);
		}

		[Test(async)]
		public function async_handlers_should_run_in_reverse_order_when_reversed():void
		{
			const results:Array = [];
			for each (var id:String in['A', 'B', 'C', 'D'])
				dispatcher.addMessageHandler(message, createAsyncHandler(results.push, id));
			dispatcher.dispatchMessage(message, null, true);
			delayAssertion(function():void {
				assertThat(results, array(['D', 'C', 'B', 'A']));
			}, 200);
		}

		[Test(async)]
		public function async_and_sync_handlers_should_run_in_order():void
		{
			const results:Array = [];
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'A'));
			dispatcher.addMessageHandler(message, createHandler(results.push, 'B'));
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'C'));
			dispatcher.addMessageHandler(message, createHandler(results.push, 'D'));
			dispatcher.dispatchMessage(message);
			delayAssertion(function():void {
				assertThat(results, array(['A', 'B', 'C', 'D']));
			}, 200);
		}

		[Test(async)]
		public function async_and_sync_handlers_should_run_in_order_when_reversed():void
		{
			const results:Array = [];
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'A'));
			dispatcher.addMessageHandler(message, createHandler(results.push, 'B'));
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'C'));
			dispatcher.addMessageHandler(message, createHandler(results.push, 'D'));
			dispatcher.dispatchMessage(message, null, true);
			delayAssertion(function():void {
				assertThat(results, array(['D', 'C', 'B', 'A']));
			}, 200);
		}

		[Test]
		public function terminated_message_should_not_reach_further_handlers():void
		{
			const results:Array = [];
			dispatcher.addMessageHandler(message, createHandler(results.push, 'A'));
			dispatcher.addMessageHandler(message, createHandler(results.push, 'B'));
			dispatcher.addMessageHandler(message, createCallbackHandlerThatErrors(results.push, 'C (with error)'));
			dispatcher.addMessageHandler(message, createHandler(results.push, 'D'));
			dispatcher.dispatchMessage(message);
			assertThat(results, array(['A', 'B', 'C (with error)']));
		}

		[Test]
		public function terminated_message_should_not_reach_further_handlers_when_reversed():void
		{
			const results:Array = [];
			dispatcher.addMessageHandler(message, createHandler(results.push, 'A'));
			dispatcher.addMessageHandler(message, createHandler(results.push, 'B'));
			dispatcher.addMessageHandler(message, createCallbackHandlerThatErrors(results.push, 'C (with error)'));
			dispatcher.addMessageHandler(message, createHandler(results.push, 'D'));
			dispatcher.dispatchMessage(message, null, true);
			assertThat(results, array(['D', 'C (with error)']));
		}

		[Test(async)]
		public function terminated_async_message_should_not_reach_further_handlers():void
		{
			const results:Array = [];
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'A'));
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'B'));
			dispatcher.addMessageHandler(message, createCallbackHandlerThatErrors(results.push, 'C (with error)'));
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'D'));
			dispatcher.dispatchMessage(message);
			delayAssertion(function():void {
				assertThat(results, array(['A', 'B', 'C (with error)']));
			}, 200);
		}

		[Test(async)]
		public function terminated_async_message_should_not_reach_further_handlers_when_reversed():void
		{
			const results:Array = [];
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'A'));
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'B'));
			dispatcher.addMessageHandler(message, createCallbackHandlerThatErrors(results.push, 'C (with error)'));
			dispatcher.addMessageHandler(message, createAsyncHandler(results.push, 'D'));
			dispatcher.dispatchMessage(message, null, true);
			delayAssertion(function():void {
				assertThat(results, array(['D', 'C (with error)']));
			}, 200);
		}

		[Test]
		public function handler_is_only_added_once():void
		{
			var callbackCount:int = 0;
			const handler:Function = function():void {
				callbackCount++;
			};
			dispatcher.addMessageHandler(message, handler);
			dispatcher.addMessageHandler(message, handler);
			dispatcher.dispatchMessage(message);
			assertThat(callbackCount, equalTo(1));
		}
		*/

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private async Task DelayAssertion(Delegate closure, int delay = 50)
		{
			await Task.Delay(delay);
			InvokeDelegate (closure);
		}

		private async Task SetTimeout(int delay, Delegate callback)
		{
			await Task.Delay(delay);
			InvokeDelegate (callback);
		}

		private void InvokeDelegate(Delegate del)
		{
			int length = del.Method.GetParameters ().Length;
			object[] parameters = new object[length];
			del.DynamicInvoke (parameters);
		}
	}
}


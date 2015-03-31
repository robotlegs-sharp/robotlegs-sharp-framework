//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using System.Threading.Tasks;
using robotlegs.bender.framework.impl.safelyCallBackSupport;
using robotlegs.bender.framework.api;
using System.Threading;
using System.Collections.Generic;

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
			dispatcher.AddMessageHandler(message, delegate(object msg, HandlerAsyncCallback callback) {
				actualMessage = msg;
				callback();
			});
			dispatcher.DispatchMessage(message);
			Assert.That(actualMessage, Is.EqualTo(message));
		}

		[Test]
		public async Task async_handler_handles_message()
		{
			object actualMessage = null;
			dispatcher.AddMessageHandler(message, delegate(object msg, HandlerAsyncCallback callback) {
				actualMessage = msg;
				SetTimeout(callback, 5, new object[]{null});
			});
			dispatcher.DispatchMessage(message);
			await Delay ();
			Assert.That(actualMessage, Is.EqualTo(message));
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
			await Delay();
			Assert.AreEqual (callbackCount, 1);
		}

		[Test]
		public async Task callback_is_called_once_after_async_handler()
		{
			int callbackCount = 0;
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler());
			dispatcher.DispatchMessage(message, delegate() {
				callbackCount++;
			});
			await Delay ();
			Assert.That(callbackCount, Is.EqualTo(1));
		}

		[Test]
		public async Task callback_is_called_once_after_sync_and_async_handlers()
		{
			int callbackCount = 0;
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler());
			dispatcher.AddMessageHandler(message, CreateHandler.Handler());
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler());
			dispatcher.AddMessageHandler(message, CreateHandler.Handler());
			dispatcher.DispatchMessage(message, delegate() {
				callbackCount++;
			});
			await Delay (100);
			Assert.That(callbackCount, Is.EqualTo(1));
		}

		[Test]
		public void handler_passes_error_to_callback()
		{
			object expectedError = "Error";
			object actualError = null;
			dispatcher.AddMessageHandler(message, delegate(object msg, HandlerAsyncCallback callback) {
				callback(expectedError);
			});
			dispatcher.DispatchMessage(message, delegate(object error) {
				actualError = error;
			});
			Assert.That(actualError, Is.EqualTo(expectedError));
		}

		[Test]
		public async void async_handler_passes_error_to_callback()
		{
			object expectedError = "Error";
			object actualError = null;
			dispatcher.AddMessageHandler(message, delegate(object msg, HandlerAsyncCallback callback) {
				SetTimeout(callback, 5, new object[]{expectedError});
			});
			dispatcher.DispatchMessage(message, delegate(object error) {
				actualError = error;
			});
			await Delay ();
			Assert.That(actualError, Is.EqualTo(expectedError));
		}

		[Test]
		public void handler_that_calls_back_more_than_once_is_ignored()
		{
			int callbackCount = 0;
			dispatcher.AddMessageHandler(message, delegate(object msg, HandlerAsyncCallback callback) {
				callback();
				callback();
			});
			dispatcher.DispatchMessage(message, delegate(object error) {
				callbackCount++;
			});
			Assert.That(callbackCount, Is.EqualTo(1));
		}

		[Test]
		public async Task async_handler_that_calls_back_more_than_once_is_ignored()
		{
			int callbackCount = 0;
			dispatcher.AddMessageHandler(message, delegate(object msg, HandlerAsyncCallback callback) {
				callback();
				callback();
			});
			dispatcher.DispatchMessage(message, delegate(object error) {
				callbackCount++;
			});
			await Delay();
			Assert.That(callbackCount, Is.EqualTo(1));
		}

		[Test]
		public void sync_handlers_should_run_in_order()
		{
			List<object> results = new List<object>();
			foreach (char id in new object[]{'A', 'B', 'C', 'D'})
				dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{id}));
			dispatcher.DispatchMessage(message);
			Assert.That (results, Is.EqualTo (new object[]{ 'A', 'B', 'C', 'D' }).AsCollection);
		}

		[Test]
		public void sync_handlers_should_run_in_reverse_order()
		{
			List<object> results = new List<object>();
			foreach (char id in new object[]{'A', 'B', 'C', 'D'})
				dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{id}));
			dispatcher.DispatchMessage(message, true);
			Assert.That (results, Is.EqualTo (new object[]{ 'D', 'C', 'B', 'A' }).AsCollection);
		}

		[Test]
		public async Task async_handlers_should_run_in_order()
		{
			//TODO: This fails but not everytime...
			List<object> results = new List<object>();
			foreach (char id in new object[]{'A', 'B', 'C', 'D'})
				dispatcher.AddMessageHandler (message, CreateHandler.AsyncHandler ((Action<object>)results.Add, new object[]{ id }));
			dispatcher.DispatchMessage(message);
			await Delay(200);
			Assert.That (results, Is.EqualTo (new object[]{ 'A', 'B', 'C', 'D' }).AsCollection);
		}

		[Test]
		public async Task async_handlers_should_run_in_reverse_order_when_reversed()
		{
			List<object> results = new List<object>();
			foreach (char id in new object[]{'A', 'B', 'C', 'D'})
				dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{id}));
			dispatcher.DispatchMessage(message, true);
			await Delay(200);
			Assert.That (results, Is.EqualTo (new object[]{ 'D', 'C', 'B', 'A' }).AsCollection);
		}

		[Test]
		public async Task async_and_sync_handlers_should_run_in_order()
		{
			//TODO: This fails but not everytime... sometimes it's empty
			List<object> results = new List<object> ();
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'A'}));
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'B'}));
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'C'}));
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'D'}));
			dispatcher.DispatchMessage(message);
			await Delay (200);
			Assert.That (results, Is.EqualTo (new object[]{ 'A', 'B', 'C', 'D' }).AsCollection);
		}

		[Test]
		public async Task async_and_sync_handlers_should_run_in_order_when_reversed()
		{
			List<object> results = new List<object> ();
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'A'}));
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'B'}));
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'C'}));
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'D'}));
			dispatcher.DispatchMessage(message, true);
			await Delay (200);
			Assert.That (results, Is.EqualTo (new object[]{ 'D', 'C', 'B', 'A' }).AsCollection);
		}

		[Test]
		public void terminated_message_should_not_reach_further_handlers()
		{
			List<object> results = new List<object> ();
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'A'}));
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'B'}));
			dispatcher.AddMessageHandler(message, CreateHandler.HandlerThatErrors((Action<object>)results.Add, new object[]{"C (with error)"}));
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'D'}));
			dispatcher.DispatchMessage(message);
			Assert.That (results, Is.EqualTo (new object[]{ 'A', 'B', "C (with error)" }).AsCollection);
		}

		[Test]
		public void terminated_message_should_not_reach_further_handlers_when_reversed()
		{
			List<object> results = new List<object> ();
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'A'}));
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'B'}));
			dispatcher.AddMessageHandler(message, CreateHandler.HandlerThatErrors((Action<object>)results.Add, new object[]{"C (with error)"}));
			dispatcher.AddMessageHandler(message, CreateHandler.Handler((Action<object>)results.Add, new object[]{'D'}));
			dispatcher.DispatchMessage(message, true);
			Assert.That (results, Is.EqualTo (new object[]{ 'D', "C (with error)" }).AsCollection);
		}

		[Test]
		public async Task terminated_async_message_should_not_reach_further_handlers()
		{
			List<object> results = new List<object> ();
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'A'}));
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'B'}));
			dispatcher.AddMessageHandler(message, CreateHandler.HandlerThatErrors((Action<object>)results.Add, new object[]{"C (with error)"}));
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'D'}));
			dispatcher.DispatchMessage(message);
			await Delay (200);
			Assert.That (results, Is.EqualTo (new object[]{ 'A', 'B', "C (with error)" }).AsCollection);
		}

		[Test]
		public async Task terminated_async_message_should_not_reach_further_handlers_when_reversed()
		{
			List<object> results = new List<object> ();
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'A'}));
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'B'}));
			dispatcher.AddMessageHandler(message, CreateHandler.HandlerThatErrors((Action<object>)results.Add, new object[]{"C (with error)"}));
			dispatcher.AddMessageHandler(message, CreateHandler.AsyncHandler((Action<object>)results.Add, new object[]{'D'}));
			dispatcher.DispatchMessage(message, true);
			await Delay (200);
			Assert.That (results, Is.EqualTo (new object[]{ 'D', "C (with error)" }).AsCollection);
		}

		[Test]
		public void handler_is_only_added_once()
		{
			int callbackCount = 0;
			Action handler = delegate() {
				callbackCount++;
			};
			dispatcher.AddMessageHandler(message, handler);
			dispatcher.AddMessageHandler(message, handler);
			dispatcher.DispatchMessage(message);
			Assert.That(callbackCount, Is.EqualTo(1));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private async Task DelayAssertion(Delegate closure, int delay = 50)
		{
			await Task.Delay(delay);
			InvokeDelegate (closure);
		}

		private void SetTimeout(Delegate callback, int delay, object[] args = null)
		{
			Timer t = new Timer(new TimerCallback(delegate(object state)
				{
					callback.DynamicInvoke(args);
				}
			), null, delay, System.Threading.Timeout.Infinite);
		}

		private void InvokeDelegate(Delegate del)
		{
			int length = del.Method.GetParameters ().Length;
			object[] parameters = new object[length];
			del.DynamicInvoke (parameters);
		}

		private Task Delay(int milliseconds = 50)
		{
			return Task.Delay(milliseconds);
		}
	}
}


//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Robotlegs.Bender.Framework.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Robotlegs.Bender.Framework.Impl
{
	[TestFixture]
	public class LifecycleTransitionTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Lifecycle lifecycle;

		private LifecycleTransition transition;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			lifecycle = new Lifecycle(new object());
			transition = new LifecycleTransition("test", lifecycle);
		}

		[TearDown]
		public void after()
		{
			lifecycle = null;
			transition = null;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void invalid_transition_throws_error()
		{
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
                transition.FromStates(LifecycleState.DESTROYED).Enter()
           ));
		}

		[Test]
		public void invalid_transition_does_not_throw_when_errorListener_is_attached()
		{
			lifecycle.ERROR += delegate(Exception error) {
			};
			transition.FromStates(LifecycleState.DESTROYED).Enter();
		}

		[Test]
		public void finalState_is_set()
		{
			transition.ToStates(LifecycleState.INITIALIZING, LifecycleState.ACTIVE).Enter();
			Assert.That (lifecycle.state, Is.EqualTo (LifecycleState.ACTIVE));
		}

		[Test]
		public void transitionState_is_set()
		{
			transition.ToStates(LifecycleState.INITIALIZING, LifecycleState.ACTIVE)
				.AddBeforeHandler(delegate(object message, HandlerAsyncCallback callback) {
				})
				.Enter();
			Assert.That(lifecycle.state, Is.EqualTo(LifecycleState.INITIALIZING));
		}

		[Test]
		public void lifecycle_events_are_dispatched()
		{
			List<string> actual = new List<string>();
			List<string> expected = new List<string>{
				"preTransition",
				"transition",
				"postTransition"};
			transition.transition += delegate () {
				actual.Add ("transition");
			};
			transition.preTransition += delegate () {
				actual.Add ("preTransition");
			};
			transition.postTransition += delegate () {
				actual.Add ("postTransition");
			};
			transition.Enter();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void when_are_in_order()
		{
			List<int> actual = new List<int>();
			List<int> expected = new List<int>{1, 2, 3};
			transition.AddWhenHandler(delegate(){
				actual.Add(1);
			});
			transition.AddWhenHandler(delegate(){
				actual.Add(2);
			});
			transition.AddWhenHandler(delegate(){
				actual.Add(3);
			});
			transition.Enter();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void when_can_be_reversed()
		{
			List<int> actual = new List<int>();
			List<int> expected = new List<int>{3, 2, 1};
			transition.AddWhenHandler(delegate(){
				actual.Add(1);
			});
			transition.AddWhenHandler(delegate(){
				actual.Add(2);
			});
			transition.AddWhenHandler(delegate(){
				actual.Add(3);
			});
			transition.InReverse ();
			transition.Enter();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void after_are_in_order()
		{
			List<int> actual = new List<int>();
			List<int> expected = new List<int>{1, 2, 3};
			transition.AddAfterHandler(delegate(){
				actual.Add(1);
			});
			transition.AddAfterHandler(delegate(){
				actual.Add(2);
			});
			transition.AddAfterHandler(delegate(){
				actual.Add(3);
			});
			transition.Enter();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void after_can_be_reversed()
		{
			List<int> actual = new List<int>();
			List<int> expected = new List<int>{3, 2, 1};
			transition.AddAfterHandler(delegate(){
				actual.Add(1);
			});
			transition.AddAfterHandler(delegate(){
				actual.Add(2);
			});
			transition.AddAfterHandler(delegate(){
				actual.Add(3);
			});
			transition.InReverse ();
			transition.Enter();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void callback_is_called()
		{
			int callCount = 0;
			transition.Enter(delegate() {
				callCount++;
			});
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void beforeHandlers_are_run()
		{
			List<string> expected = new List<string>{"a", "b", "c"};
			List<string> actual = new List<string>();
			transition.AddBeforeHandler(delegate(){
				actual.Add("a");
			});
			transition.AddBeforeHandler(delegate(){
				actual.Add("b");
			});
			transition.AddBeforeHandler(delegate(){
				actual.Add("c");
			});
			transition.Enter();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void beforeHandlers_are_run_in_reverse()
		{
			List<string> expected = new List<string>{"c", "b", "a"};
			List<string> actual = new List<string>();
			transition.InReverse();
			transition.AddBeforeHandler(delegate(){
				actual.Add("a");
			});
			transition.AddBeforeHandler(delegate(){
				actual.Add("b");
			});
			transition.AddBeforeHandler(delegate(){
				actual.Add("c");
			});
			transition.Enter();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void beforeHandler_error_throws()
		{
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
                transition.AddBeforeHandler(delegate (object message, HandlerAsyncCallback callback) {
                    callback(new Exception("some error message"));
                }).Enter()
                ));
		}

		[Test]
		public void beforeHandler_does_not_throw_when_errorListener_is_attached()
		{
			Exception expected = new Exception("There was a problem");
			Exception actual = null;
			lifecycle.ERROR += delegate(Exception exception) {
			};
			transition.AddBeforeHandler(delegate(object message, HandlerAsyncCallback callback){
				callback(expected);
			}).Enter(delegate(Exception error) {
				actual = error;
			});
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void invalidTransition_is_passed_to_callback_when_errorListener_is_attached()
		{
			object actual = null;
			lifecycle.ERROR += delegate(Exception error){
			};
			transition.FromStates(LifecycleState.DESTROYING).Enter(delegate(Exception error){
				actual = error;
			});
			Assert.That (actual, Is.Not.Null);
		}

		[Test]
		public void beforeHandlerError_reverts_state()
		{
			LifecycleState expected = lifecycle.state;
			lifecycle.ERROR += delegate(Exception error){
			};
			transition.FromStates(LifecycleState.UNINITIALIZED)
				.ToStates(LifecycleState.INITIALIZING, LifecycleState.ACTIVE)
				.AddBeforeHandler(delegate(object message, HandlerAsyncCallback callback) {
					callback("There was a problem");
				}).Enter();
			Assert.That(lifecycle.state, Is.EqualTo(expected));
		}

		[Test]
		public void callback_is_called_if_already_transitioned()
		{
			int callCount = 0;
			transition.FromStates(LifecycleState.UNINITIALIZED).ToStates(LifecycleState.INITIALIZING, LifecycleState.ACTIVE);
			transition.Enter();
			transition.Enter(delegate() {
				callCount++;
			});
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public async Task callback_added_during_transition_is_called()
		{
			int callCount = 0;
			transition.FromStates(LifecycleState.UNINITIALIZED)
				.ToStates(LifecycleState.INITIALIZING, LifecycleState.ACTIVE)
				.AddBeforeHandler(delegate(object message, HandlerAsyncCallback callback) {
					new Timer (new TimerCallback (callback), null, 1, System.Threading.Timeout.Infinite);
				});
			transition.Enter();
			transition.Enter(delegate() {
				callCount++;
			});
			Assert.That (callCount, Is.EqualTo (0));
			await Task.Delay (50);
			Assert.That(callCount, Is.EqualTo(1));
		}
	}
}


using System;
using NUnit.Framework;
using robotlegs.bender.framework.api;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
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
			lifecycle = new Lifecycle();
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
		[ExpectedException]
		public void invalid_transition_throws_error()
		{
			transition.FromStates(LifecycleState.DESTROYED).Enter();
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
				.AddBeforeHandler(delegate(object message, MessageDispatcher.HandlerAsyncCallback callback) {
					//setTimeout(callback, 1);
				})
				.Enter();
			Assert.That(lifecycle.state, Is.EqualTo(LifecycleState.INITIALIZING));
		}

//		[Test]
//		public void lifecycle_events_are_dispatched()
//		{
//			const actual:Array = [];
//			const expected:Array = [
//				LifecycleEvent.PRE_INITIALIZE,
//				LifecycleEvent.INITIALIZE,
//				LifecycleEvent.POST_INITIALIZE];
//			transition.withEvents(expected[0], expected[1], expected[2]);
//			for each (var type:String in expected)
//			{
//				lifecycle.addEventListener(type, function(event:Event):void {
//					actual.push(event.type);
//				});
//			}
//			transition.enter();
//			assertThat(actual, array(expected));
//		}

//		[Test]
//		public void listeners_are_reversed()
//		{
//			const actual:Array = [];
//			const expected:Array = [3, 2, 1];
//			transition.withEvents("preEvent", "event", "postEvent").inReverse();
//			lifecycle.addEventListener("event", function(event:Event):void {
//				actual.push(1);
//			});
//			lifecycle.addEventListener("event", function(event:Event):void {
//				actual.push(2);
//			});
//			lifecycle.addEventListener("event", function(event:Event):void {
//				actual.push(3);
//			});
//			transition.enter();
//			assertThat(actual, array(expected));
//		}

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
		[ExpectedException]
		public void beforeHandler_error_throws()
		{
			transition.AddBeforeHandler (delegate(object message, MessageDispatcher.HandlerAsyncCallback callback) {
				callback (new Exception ("some error message"));
			}).Enter ();
		}

		[Test]
		public void beforeHandler_does_not_throw_when_errorListener_is_attached()
		{
			Exception expected = new Exception("There was a problem");
			Exception actual = null;
			lifecycle.ERROR += delegate(Exception exception) {
			};
			transition.AddBeforeHandler(delegate(object message, MessageDispatcher.HandlerAsyncCallback callback){
				callback(expected);
			}).Enter(delegate(Exception error) {
				actual = error;
			});
			Console.WriteLine ("Actual: " + actual);
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
				.AddBeforeHandler(delegate(object message, MessageDispatcher.HandlerAsyncCallback callback) {
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

		/*
		[Test(async)]
		public function callback_added_during_transition_is_called():void
		{
			var callCount:int = 0;
			transition.fromStates(LifecycleState.UNINITIALIZED)
				.toStates("startState", "endState")
				.addBeforeHandler(function(message:Object, callback:Function):void {
					setTimeout(callback, 1);
				});
			transition.enter();
			transition.enter(function():void {
				callCount++;
			});
			Async.delayCall(this, function():void {
				assertThat(callCount, equalTo(1));
			}, 50);	
		}
		*/
	}
}


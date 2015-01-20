using System;
using NUnit.Framework;
using robotlegs.bender.framework.api;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
{
	[TestFixture]
	public class LifecycleTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Lifecycle lifecycle;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			lifecycle = new Lifecycle();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void lifecycle_starts_uninitialized()
		{
			Assert.AreEqual (LifecycleState.UNINITIALIZED, lifecycle.state);
			Assert.True (lifecycle.Uninitialized);
		}

		// ----- Basic valid transitions

		[Test]
		public void initialize_turns_state_active()
		{
			lifecycle.Initialize();
			Assert.AreEqual (LifecycleState.ACTIVE, lifecycle.state);
			Assert.True (lifecycle.Active);
		}

		[Test]
		public void suspend_turns_state_suspended()
		{
			lifecycle.Initialize();
			lifecycle.Suspend();
			Assert.That(lifecycle.state, Is.EqualTo(LifecycleState.SUSPENDED));
			Assert.That(lifecycle.Suspended, Is.True);
		}

		[Test]
		public void resume_turns_state_active()
		{
			lifecycle.Initialize();
			lifecycle.Suspend();
			lifecycle.Resume();
			Assert.That(lifecycle.state, Is.EqualTo(LifecycleState.ACTIVE));
			Assert.That(lifecycle.Active, Is.True);
		}

		[Test]
		public void destroy_turns_state_destroyed()
		{
			lifecycle.Initialize();
			lifecycle.Destroy();
			Assert.That(lifecycle.state, Is.EqualTo(LifecycleState.DESTROYED));
			Assert.That(lifecycle.Destroyed, Is.True);
		}

		[Test]
		public void typical_transition_chain_does_not_throw_errors()
		{
			Delegate[] methods = new Delegate[]{
				(Action<Action>)lifecycle.Initialize,
				(Action<Action>)lifecycle.Suspend,
				(Action<Action>)lifecycle.Resume,
				(Action<Action>)lifecycle.Suspend,
				(Action<Action>)lifecycle.Resume,
				(Action<Action>)lifecycle.Destroy};
			Assert.That(MethodErrorCount(methods), Is.EqualTo(0));
		}

		// ----- Events

//		/*
		[Test]
		public void events_are_dispatched()
		{
			List<object> actual = new List<object> ();
			List<object> expected = new List<object>{ 
				"PRE_INITIALIZE",
				"INITIALIZE",
				"POST_INITIALIZE",
				"PRE_SUSPEND",
				"SUSPEND",
				"POST_SUSPEND",
				"PRE_RESUME",
				"RESUME",
				"POST_RESUME",
				"PRE_DESTROY",
				"DESTROY",
				"POST_DESTROY"
			};
			lifecycle.PRE_INITIALIZE += CreateValuePusher (actual, "PRE_INITIALIZE");
			lifecycle.INITIALIZE += CreateValuePusher (actual, "INITIALIZE");
			lifecycle.POST_INITIALIZE += CreateValuePusher (actual, "POST_INITIALIZE");
			lifecycle.PRE_SUSPEND += CreateValuePusher (actual, "PRE_SUSPEND");
			lifecycle.SUSPEND += CreateValuePusher (actual, "SUSPEND");
			lifecycle.POST_SUSPEND += CreateValuePusher (actual, "POST_SUSPEND");
			lifecycle.PRE_RESUME += CreateValuePusher (actual, "PRE_RESUME");
			lifecycle.RESUME += CreateValuePusher (actual, "RESUME");
			lifecycle.POST_RESUME += CreateValuePusher (actual, "POST_RESUME");
			lifecycle.PRE_DESTROY += CreateValuePusher (actual, "PRE_DESTROY");
			lifecycle.DESTROY += CreateValuePusher (actual, "DESTROY");
			lifecycle.POST_DESTROY += CreateValuePusher (actual, "POST_DESTROY");

			lifecycle.Initialize();
			lifecycle.Suspend();
			lifecycle.Resume();
			lifecycle.Destroy();

			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

//		*/

		// ----- Shorthand transition handlers

//		[Test]
//		public void when_and_afterHandlers_with_single_arguments_receive_event_types()
//		{
//			const expected:Array = [
//				LifecycleEvent.INITIALIZE, LifecycleEvent.POST_INITIALIZE,
//				LifecycleEvent.SUSPEND, LifecycleEvent.POST_SUSPEND,
//				LifecycleEvent.RESUME, LifecycleEvent.POST_RESUME,
//				LifecycleEvent.Destroy, LifecycleEvent.POST_DESTROY];
//			const actual:Array = [];
//			const handler:Function = function(type:String) {
//				actual.push(type);
//			};
//			lifecycle
//				.WhenInitializing(handler).AfterInitializing(handler)
//				.WhenSuspending(handler).AfterSuspending(handler)
//				.WhenResuming(handler).AfterResuming(handler)
//				.WhenDestroying(handler).AfterDestroying(handler);
//			lifecycle.Initialize();
//			lifecycle.Suspend();
//			lifecycle.Resume();
//			lifecycle.Destroy();
//			Assert.That(actual, array(expected));
//		}

		[Test]
		public void when_and_afterHandlers_with_no_arguments_are_called()
		{
			int callCount = 0;
			Action handler = delegate() {
				callCount++;
			};
			lifecycle
				.WhenInitializing(handler).AfterInitializing(handler)
				.WhenSuspending(handler).AfterSuspending(handler)
				.WhenResuming(handler).AfterResuming(handler)
				.WhenDestroying(handler).AfterDestroying(handler);
			lifecycle.Initialize();
			lifecycle.Suspend();
			lifecycle.Resume();
			lifecycle.Destroy();
			Assert.That(callCount, Is.EqualTo(8));
		}

		[Test]
		public void before_handlers_are_executed()
		{
			int callCount = 0;
			Action handler = delegate() {
				callCount++;
			};
			lifecycle
				.BeforeInitializing(handler)
				.BeforeSuspending(handler)
				.BeforeResuming(handler)
				.BeforeDestroying(handler);
			lifecycle.Initialize();
			lifecycle.Suspend();
			lifecycle.Resume();
			lifecycle.Destroy();
			Assert.That(callCount, Is.EqualTo(4));
		}

		/*
		[Test(async)]
		public void async_before_handlers_are_executed()
		{
			var callCount:int = 0;
			const handler:Function = function(message:Object, callback:Function) {
				callCount++;
				setTimeout(callback, 1);
			};
			lifecycle
				.BeforeInitializing(handler)
				.BeforeSuspending(handler)
				.BeforeResuming(handler)
				.BeforeDestroying(handler);
			lifecycle.Initialize(function() {
				lifecycle.Suspend(function() {
					lifecycle.Resume(function() {
						lifecycle.Destroy();
					})
				})
			});
			Async.delayCall(this, function() {
				Assert.That(callCount, Is.EqualTo(4));
			}, 200);
		}
		*/

		// ----- Suspend and Destroy run backwards

		[Test]
		public void suspend_runs_backwards()
		{
			List<object> actual = new List<object>();
			List<object> expected = new List<object> () {
				"before3", "before2", "before1",
				"when3", "when2", "when1",
				"after3", "after2", "after1"
			};
			lifecycle.BeforeSuspending(CreateValuePusher(actual, "before1"));
			lifecycle.BeforeSuspending(CreateValuePusher(actual, "before2"));
			lifecycle.BeforeSuspending(CreateValuePusher(actual, "before3"));
			lifecycle.WhenSuspending(CreateValuePusher(actual, "when1"));
			lifecycle.WhenSuspending(CreateValuePusher(actual, "when2"));
			lifecycle.WhenSuspending(CreateValuePusher(actual, "when3"));
			lifecycle.AfterSuspending(CreateValuePusher(actual, "after1"));
			lifecycle.AfterSuspending(CreateValuePusher(actual, "after2"));
			lifecycle.AfterSuspending(CreateValuePusher(actual, "after3"));
			lifecycle.Initialize();
			lifecycle.Suspend();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void destroy_runs_backwards()
		{
			List<object> actual = new List<object>();
			List<object> expected = new List<object> () {
				"before3", "before2", "before1",
				"when3", "when2", "when1",
				"after3", "after2", "after1"
			};
			lifecycle.BeforeDestroying(CreateValuePusher(actual, "before1"));
			lifecycle.BeforeDestroying(CreateValuePusher(actual, "before2"));
			lifecycle.BeforeDestroying(CreateValuePusher(actual, "before3"));
			lifecycle.WhenDestroying(CreateValuePusher(actual, "when1"));
			lifecycle.WhenDestroying(CreateValuePusher(actual, "when2"));
			lifecycle.WhenDestroying(CreateValuePusher(actual, "when3"));
			lifecycle.AfterDestroying(CreateValuePusher(actual, "after1"));
			lifecycle.AfterDestroying(CreateValuePusher(actual, "after2"));
			lifecycle.AfterDestroying(CreateValuePusher(actual, "after3"));
			lifecycle.Initialize();
			lifecycle.Destroy();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		// ----- Before handlers callback message

//		[Test]
//		public void beforeHandler_callbacks_are_passed_correct_message()
//		{
//			const expected:Array = [
//				LifecycleEvent.PRE_INITIALIZE, LifecycleEvent.INITIALIZE, LifecycleEvent.POST_INITIALIZE,
//				LifecycleEvent.PRE_SUSPEND, LifecycleEvent.SUSPEND, LifecycleEvent.POST_SUSPEND,
//				LifecycleEvent.PRE_RESUME, LifecycleEvent.RESUME, LifecycleEvent.POST_RESUME,
//				LifecycleEvent.PRE_DESTROY, LifecycleEvent.Destroy, LifecycleEvent.POST_DESTROY];
//			const actual:Array = [];
//			lifecycle.BeforeInitializing(CreateMessagePusher(actual));
//			lifecycle.WhenInitializing(CreateMessagePusher(actual));
//			lifecycle.AfterInitializing(CreateMessagePusher(actual));
//			lifecycle.BeforeSuspending(CreateMessagePusher(actual));
//			lifecycle.WhenSuspending(CreateMessagePusher(actual));
//			lifecycle.AfterSuspending(CreateMessagePusher(actual));
//			lifecycle.BeforeResuming(CreateMessagePusher(actual));
//			lifecycle.WhenResuming(CreateMessagePusher(actual));
//			lifecycle.BeforeResuming(CreateMessagePusher(actual));
//			lifecycle.BeforeDestroying(CreateMessagePusher(actual));
//			lifecycle.WhenDestroying(CreateMessagePusher(actual));
//			lifecycle.AfterDestroying(CreateMessagePusher(actual));
//			lifecycle.Initialize();
//			lifecycle.Suspend();
//			lifecycle.Resume();
//			lifecycle.Destroy();
//			Assert.That(actual, array(expected));
//		}

		// ----- StateChange Event

		[Test]
		public void stateChange_triggers_event()
		{
			bool fired = false;
			lifecycle.STATE_CHANGE += delegate() {
				fired = true;
			};
			lifecycle.Initialize();
			Assert.That(fired, Is.True);
		}

		// ----- Adding handlers that will never be called

		[Test]
		[ExpectedException("robotlegs.bender.framework.api.LifecycleException")]
		public void adding_BeforeInitializing_handler_after_initialization_throws_error()
		{
			lifecycle.Initialize();
			lifecycle.BeforeInitializing(nop);
		}

		[Test]
		[ExpectedException("robotlegs.bender.framework.api.LifecycleException")]
		public void adding_WhenInitializing_handler_after_initialization_throws_error()
		{
			lifecycle.Initialize();
			lifecycle.WhenInitializing(nop);
		}

		/*
		[Test(async, timeout='200')]
		public void adding_WhenInitializing_handler_during_initialization_does_NOT_throw_error()
		{
			var callCount:int = 0;
			lifecycle.BeforeInitializing(function(message:Object, callback:Function) {
				setTimeout(callback, 100);
			});
			lifecycle.Initialize();
			lifecycle.WhenInitializing(function() {
				callCount++;
				Assert.That(callCount, Is.EqualTo(1));
			});
		}
		*/

		[Test]
		[ExpectedException("robotlegs.bender.framework.api.LifecycleException")]
		public void adding_AfterInitializing_handler_after_initialization_throws_error()
		{
			lifecycle.Initialize();
			lifecycle.AfterInitializing(nop);
		}

		[Test]
		public void adding_AfterInitializing_handler_during_initialization_does_NOT_throw_error()
		{
			int callCount = 0;
			lifecycle.WhenInitializing(delegate() {
				lifecycle.AfterInitializing(delegate() {
					callCount++;
				});
			});
			lifecycle.Initialize();
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void TestCheck()
		{
			Console.WriteLine ("Begin");
			lifecycle.BeforeInitializing (delegate {
				Console.WriteLine ("Before1");
			});
			lifecycle.BeforeInitializing (delegate {
				Console.WriteLine ("Before2");
			});
			lifecycle.BeforeInitializing (delegate {
				Console.WriteLine ("Before3");
			});
			lifecycle.WhenInitializing (delegate {
				Console.WriteLine ("When1");
			});
			lifecycle.WhenInitializing (delegate {
				Console.WriteLine ("When2");
			});
			lifecycle.WhenInitializing (delegate {
				Console.WriteLine ("When3");
			});
			lifecycle.AfterInitializing (delegate {
				Console.WriteLine ("After1");
			});
			lifecycle.AfterInitializing (delegate {
				Console.WriteLine ("After2");
			});
			lifecycle.AfterInitializing (delegate {
				Console.WriteLine ("After3");
			});
			lifecycle.Initialize ();
			Console.WriteLine ("End");
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private int MethodErrorCount(Delegate[] methods)
		{
			int errorCount = 0;
			foreach (Delegate method in methods)
			{
				try
				{
					object[] args = new object[method.Method.GetParameters().Length];
					method.DynamicInvoke(args);
				}
				catch (Exception error)
				{
					Console.WriteLine (error.Message);
					errorCount++;
				}
			}
			return errorCount;
		}

		private Action CreateValuePusher(List<object> list, object value)
		{
			return delegate() {
				list.Add(value);
			};
		}

		private Action<object> CreateMessagePusher(List<object> list)
		{
			return delegate(object message) {
				list.Add(message);
			};
		}

		private void nop()
		{
		}
	}
}


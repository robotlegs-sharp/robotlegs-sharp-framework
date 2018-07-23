//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using System;
using NUnit.Framework;
using Robotlegs.Bender.Framework.API;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Robotlegs.Bender.Framework.Impl
{
    [TestFixture]
    public class LifecycleTest
    {
        /*============================================================================*/
        /* Private Properties                                                         */
        /*============================================================================*/

        #region Fields

        private Lifecycle lifecycle;
        private object target;

        #endregion Fields

        /*============================================================================*/
        /* Test Setup and Teardown                                                    */
        /*============================================================================*/

        #region Methods

        [Test]
        public void adding_AfterInitializing_handler_after_initialization_throws_error()
        {
            Assert.Throws(typeof(LifecycleException), new TestDelegate(() =>
            {
                lifecycle.Initialize();
                lifecycle.AfterInitializing(nop);
            }
            ));
        }

        [Test]
        public void adding_AfterInitializing_handler_during_initialization_does_NOT_throw_error()
        {
            int callCount = 0;
            lifecycle.WhenInitializing(delegate ()
            {
                lifecycle.AfterInitializing(delegate ()
                {
                    callCount++;
                });
            });
            lifecycle.Initialize();
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void adding_BeforeInitializing_handler_after_initialization_throws_error()
        {
            Assert.Throws(typeof(LifecycleException), new TestDelegate(() =>
            {
                lifecycle.Initialize();
                lifecycle.BeforeInitializing(nop);
            }
            ));
        }

        // ----- Adding handlers that will never be called
        [Test]
        public void adding_WhenInitializing_handler_after_initialization_throws_error()
        {
            Assert.Throws(typeof(LifecycleException), new TestDelegate(() =>
            {
                lifecycle.Initialize();
                lifecycle.WhenInitializing(nop);
            }
            ));
        }

        [Test]
        public async Task adding_WhenInitializing_handler_during_initialization_does_NOT_throw_error()
        {
            int callCount = 0;
            lifecycle.BeforeInitializing(delegate (object message, HandlerAsyncCallback callback)
            {
                Timer t = new Timer(new TimerCallback(callback), null, 100, System.Threading.Timeout.Infinite);
            });
            lifecycle.Initialize();
            Assert.That(lifecycle.state, Is.EqualTo(LifecycleState.INITIALIZING));
            lifecycle.WhenInitializing(delegate ()
            {
                callCount++;
            });

            await Task.Delay(100);
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public async Task async_before_handlers_are_executed()
        {
            int callCount = 0;
            HandlerMessageCallbackDelegate handler = delegate (object message, HandlerAsyncCallback callback)
            {
                callCount++;
                new Timer(new TimerCallback(callback), null, 1, System.Threading.Timeout.Infinite);
            };
            lifecycle
                .BeforeInitializing(handler)
                .BeforeSuspending(handler)
                .BeforeResuming(handler)
                .BeforeDestroying(handler);
            lifecycle.Initialize(delegate ()
            {
                lifecycle.Suspend(delegate ()
                {
                    lifecycle.Resume(delegate ()
                    {
                        lifecycle.Destroy();
                    });
                });
            });

            await Task.Delay(200);
            Assert.That(callCount, Is.EqualTo(4));
        }

        [Test]
        public void before_handlers_are_executed()
        {
            int callCount = 0;
            Action handler = delegate ()
            {
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

        [Test]
        public void destroy_runs_backwards()
        {
            List<object> actual = new List<object>();
            List<object> expected = new List<object>() {
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

        [Test]
        public void destroy_turns_state_destroyed()
        {
            lifecycle.Initialize();
            lifecycle.Destroy();
            Assert.That(lifecycle.state, Is.EqualTo(LifecycleState.DESTROYED));
            Assert.That(lifecycle.Destroyed, Is.True);
        }

        [Test]
        public void events_are_dispatched()
        {
            List<object> actual = new List<object>();
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
            lifecycle.PRE_INITIALIZE += CreateObjectValuePusher(actual, "PRE_INITIALIZE");
            lifecycle.INITIALIZE += CreateObjectValuePusher(actual, "INITIALIZE");
            lifecycle.POST_INITIALIZE += CreateObjectValuePusher(actual, "POST_INITIALIZE");
            lifecycle.PRE_SUSPEND += CreateObjectValuePusher(actual, "PRE_SUSPEND");
            lifecycle.SUSPEND += CreateObjectValuePusher(actual, "SUSPEND");
            lifecycle.POST_SUSPEND += CreateObjectValuePusher(actual, "POST_SUSPEND");
            lifecycle.PRE_RESUME += CreateObjectValuePusher(actual, "PRE_RESUME");
            lifecycle.RESUME += CreateObjectValuePusher(actual, "RESUME");
            lifecycle.POST_RESUME += CreateObjectValuePusher(actual, "POST_RESUME");
            lifecycle.PRE_DESTROY += CreateObjectValuePusher(actual, "PRE_DESTROY");
            lifecycle.DESTROY += CreateObjectValuePusher(actual, "DESTROY");
            lifecycle.POST_DESTROY += CreateObjectValuePusher(actual, "POST_DESTROY");

            lifecycle.Initialize();
            lifecycle.Suspend();
            lifecycle.Resume();
            lifecycle.Destroy();

            Assert.That(actual, Is.EqualTo(expected).AsCollection);
        }

        [Test]
        public void initialize_turns_state_active()
        {
            lifecycle.Initialize();
            Assert.AreEqual(LifecycleState.ACTIVE, lifecycle.state);
            Assert.True(lifecycle.Active);
        }

        [Test]
        public void lifecycle_starts_uninitialized()
        {
            Assert.AreEqual(LifecycleState.UNINITIALIZED, lifecycle.state);
            Assert.True(lifecycle.Uninitialized);
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

        [SetUp]
        public void Setup()
        {
            target = new object();
            lifecycle = new Lifecycle(target);
        }

        /*============================================================================*/
        /* Tests                                                                      */
        /*============================================================================*/

        [Test]
        public void stateChange_triggers_event()
        {
            bool fired = false;
            lifecycle.STATE_CHANGE += delegate ()
            {
                fired = true;
            };
            lifecycle.Initialize();
            Assert.That(fired, Is.True);
        }

        [Test]
        public void suspend_runs_backwards()
        {
            List<object> actual = new List<object>();
            List<object> expected = new List<object>() {
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

        // ----- Basic valid transitions
        [Test]
        public void suspend_turns_state_suspended()
        {
            lifecycle.Initialize();
            lifecycle.Suspend();
            Assert.That(lifecycle.state, Is.EqualTo(LifecycleState.SUSPENDED));
            Assert.That(lifecycle.Suspended, Is.True);
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
        // ----- Shorthand transition handlers

        // [Test] public void when_and_afterHandlers_with_single_arguments_receive_event_types() {
        // const expected:Array = [ LifecycleEvent.INITIALIZE, LifecycleEvent.POST_INITIALIZE,
        // LifecycleEvent.SUSPEND, LifecycleEvent.POST_SUSPEND, LifecycleEvent.RESUME,
        // LifecycleEvent.POST_RESUME, LifecycleEvent.Destroy, LifecycleEvent.POST_DESTROY]; const
        // actual:Array = []; const handler:Function = function(type:String) { actual.push(type); };
        // lifecycle .WhenInitializing(handler).AfterInitializing(handler)
        // .WhenSuspending(handler).AfterSuspending(handler)
        // .WhenResuming(handler).AfterResuming(handler)
        // .WhenDestroying(handler).AfterDestroying(handler); lifecycle.Initialize();
        // lifecycle.Suspend(); lifecycle.Resume(); lifecycle.Destroy(); Assert.That(actual,
        // array(expected)); }

        [Test]
        public void when_and_afterHandlers_with_no_arguments_are_called()
        {
            int callCount = 0;
            Action handler = delegate ()
            {
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

        // ----- Suspend and Destroy run backwards
        // ----- Before handlers callback message

        // [Test] public void beforeHandler_callbacks_are_passed_correct_message() { List<object>
        // expected = new List<object>(); const expected:Array = [ LifecycleEvent.PRE_INITIALIZE,
        // LifecycleEvent.INITIALIZE, LifecycleEvent.POST_INITIALIZE, LifecycleEvent.PRE_SUSPEND,
        // LifecycleEvent.SUSPEND, LifecycleEvent.POST_SUSPEND, LifecycleEvent.PRE_RESUME,
        // LifecycleEvent.RESUME, LifecycleEvent.POST_RESUME, LifecycleEvent.PRE_DESTROY,
        // LifecycleEvent.Destroy, LifecycleEvent.POST_DESTROY]; List<object> actual = new List<object>();

        // lifecycle.BeforeInitializing(CreateMessagePusher(actual));
        // lifecycle.WhenInitializing(CreateMessagePusher(actual));
        // lifecycle.AfterInitializing(CreateMessagePusher(actual));
        // lifecycle.BeforeSuspending(CreateMessagePusher(actual));
        // lifecycle.WhenSuspending(CreateMessagePusher(actual));
        // lifecycle.AfterSuspending(CreateMessagePusher(actual));
        // lifecycle.BeforeResuming(CreateMessagePusher(actual));
        // lifecycle.WhenResuming(CreateMessagePusher(actual));
        // lifecycle.BeforeResuming(CreateMessagePusher(actual));
        // lifecycle.BeforeDestroying(CreateMessagePusher(actual));
        // lifecycle.WhenDestroying(CreateMessagePusher(actual));
        // lifecycle.AfterDestroying(CreateMessagePusher(actual)); lifecycle.Initialize();
        // lifecycle.Suspend(); lifecycle.Resume(); lifecycle.Destroy(); Assert.That(actual,
        // Is.EqualTo(expected).AsCollection); }

        // ----- StateChange Event
        /*============================================================================*/
        /* Private Functions                                                          */
        /*============================================================================*/

        private HandlerMessageDelegate CreateMessagePusher(List<object> list)
        {
            return delegate (object message)
            {
                list.Add(message);
            };
        }

        private Action<object> CreateObjectValuePusher(List<object> list, object value)
        {
            return delegate (object obj)
            {
                list.Add(value);
            };
        }

        private Action CreateValuePusher(List<object> list, object value)
        {
            return delegate ()
            {
                list.Add(value);
            };
        }

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
                catch (Exception)
                {
                    errorCount++;
                }
            }
            return errorCount;
        }

        private void nop()
        {
        }

        #endregion Methods
    }
}
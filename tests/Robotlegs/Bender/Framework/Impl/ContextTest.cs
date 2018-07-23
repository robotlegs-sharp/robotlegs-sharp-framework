//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl.ContextSupport;
using Robotlegs.Bender.Framework.Impl.LoggingSupport;

namespace Robotlegs.Bender.Framework.Impl
{
    [TestFixture]
    public class ContextTest
    {
        /*============================================================================*/
        /* Private Properties                                                         */
        /*============================================================================*/

        #region Fields

        private IContext context;

        #endregion Fields

        /*============================================================================*/
        /* Test Setup and Teardown                                                    */
        /*============================================================================*/

        #region Methods

        [Test]
        public void addChild_logs_warning_if_child_parentInjector_is_already_set()
        {
            LogParams? warning = null;
            context.AddLogTarget(new CallbackLogTarget(
                delegate (LogParams log)
                {
                    if (log.level == LogLevel.WARN)
                        warning = log;
                }));
            Context child = new Context();
            child.injector.parent = new RobotlegsInjector();
            context.AddChild(child);

            Assert.That(warning, Is.Not.Null);
            Assert.That(warning.Value.message, Does.Contain("must not have a parent Injector"));
            Assert.That(warning.Value.messageParameters, Is.EqualTo(new object[] { child }).AsCollection);
        }

        [Test]
        public void addChild_logs_warning_unless_child_is_uninitialized()
        {
            LogParams? warning = null;
            context.AddLogTarget(new CallbackLogTarget(
                delegate (LogParams log)
                {
                    if (log.level == LogLevel.WARN)
                        warning = log;
                }));
            Context child = new Context();
            child.Initialize();
            context.AddChild(child);
            Assert.That(warning, Is.Not.Null);
            Assert.That(warning.Value.message, Does.Contain("must be uninitialized"));
            Assert.That(warning.Value.messageParameters, Is.EqualTo(new object[] { child }).AsCollection);
        }

        [Test]
        public void addChild_sets_child_parentInjector()
        {
            Context child = new Context();
            context.AddChild(child);
            Assert.That(child.injector.parent, Is.EqualTo(context.injector));
        }

        [Test]
        public void adding_after_initializing_hander_during_configure_is_allowed()
        {
            bool hasDoneCallback = false;
            context.Configure(new CallbackConfig(delegate
            {
                context.AfterInitializing(delegate
                {
                    hasDoneCallback = true;
                });
            }));
            context.Initialize();
            Assert.That(hasDoneCallback, Is.True);
        }

        [Test]
        public void adding_BeforeInitializing_handler_after_initialization_catches_error()
        {
            bool caught = false;
            context.ERROR += delegate (Exception obj)
            {
                caught = true;
            };
            context.Initialize();
            context.BeforeInitializing(nop);
            Assert.That(caught, Is.True);
        }

        [Test]
        public void adding_BeforeInitializing_handler_after_initialization_throws_error()
        {
            Assert.Throws(typeof(LifecycleException), new TestDelegate(() =>
            {
                context.Initialize();
                context.BeforeInitializing(nop);
            }
            ));
        }

        [Test]
        public void adding_WhenInitializing_handler_after_initialization_catches_error()
        {
            bool caught = false;
            context.ERROR += delegate (Exception obj)
            {
                caught = true;
            };
            context.Initialize();
            context.WhenInitializing(nop);
            Assert.That(caught, Is.True);
        }

        [Test]
        public void adding_WhenInitializing_handler_after_initialization_throws_error()
        {
            Assert.Throws(typeof(LifecycleException), new TestDelegate(() =>
            {
                context.Initialize();
                context.WhenInitializing(nop);
            }
            ));
        }

        [SetUp]
        public void before()
        {
            context = new Context();
        }

        /*============================================================================*/
        /* Tests                                                                      */
        /*============================================================================*/

        [Test]
        public void can_instantiate()
        {
            Assert.That(context, Is.Not.Null);
            Assert.That(context, Is.InstanceOf<IContext>());
        }

        [Test]
        public void child_is_removed_when_child_is_destroyed()
        {
            Context child = new Context();
            context.AddChild(child);
            child.Initialize();
            child.Destroy();
            Assert.That(child.injector.parent, Is.Null);
        }

        [Test]
        public void children_are_removed_when_parent_is_destroyed()
        {
            Context child1 = new Context();
            Context child2 = new Context();
            context.AddChild(child1);
            context.AddChild(child2);
            context.Initialize();
            context.Destroy();
            Assert.That(child1.injector.parent, Is.Null);
            Assert.That(child2.injector.parent, Is.Null);
        }

        [Test]
        public void configs_are_installed()
        {
            bool installed = false;
            IConfig config = new CallbackConfig(
                delegate ()
                {
                    installed = true;
                });
            context.Configure(config);
            context.Initialize();
            Assert.That(installed, Is.True);
        }

        [Test]
        public void destroy_callback_is_fired()
        {
            context.Initialize();
            context.Destroy(delegate
            {
                Assert.Pass();
            });
            Assert.Fail();
        }

        [Test]
        public void detain_stores_the_instance()
        {
            object expected = new object();
            object actual = null;
            context.Detained += delegate (object obj)
            {
                actual = obj;
            };
            context.Detain(expected);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void extensions_are_installed()
        {
            IContext actual = null;
            IExtension extension = new CallbackExtension(
                delegate (IContext c)
                {
                    actual = c;
                });
            context.Install(extension);
            Assert.That(actual, Is.EqualTo(context));
        }

        [Test]
        public void initialization_callback_is_fired()
        {
            context.Initialize(delegate
            {
                Assert.Pass();
            });
            Assert.Fail();
        }

        [Test]
        public void injector_is_mapped_into_itself()
        {
            IInjector injector = context.injector.GetInstance<IInjector>();
            Assert.That(injector, Is.EqualTo(context.injector));
        }

        [Test]
        public void lifecycleEvents_are_propagated()
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
            context.PRE_INITIALIZE += CreateValuePusher(actual, "PRE_INITIALIZE");
            context.INITIALIZE += CreateValuePusher(actual, "INITIALIZE");
            context.POST_INITIALIZE += CreateValuePusher(actual, "POST_INITIALIZE");
            context.PRE_SUSPEND += CreateValuePusher(actual, "PRE_SUSPEND");
            context.SUSPEND += CreateValuePusher(actual, "SUSPEND");
            context.POST_SUSPEND += CreateValuePusher(actual, "POST_SUSPEND");
            context.PRE_RESUME += CreateValuePusher(actual, "PRE_RESUME");
            context.RESUME += CreateValuePusher(actual, "RESUME");
            context.POST_RESUME += CreateValuePusher(actual, "POST_RESUME");
            context.PRE_DESTROY += CreateValuePusher(actual, "PRE_DESTROY");
            context.DESTROY += CreateValuePusher(actual, "DESTROY");
            context.POST_DESTROY += CreateValuePusher(actual, "POST_DESTROY");

            context.Initialize();
            context.Suspend();
            context.Resume();
            context.Destroy();
            Assert.That(actual, Is.EqualTo(expected).AsCollection);
        }

        [Test]
        public void lifecycleStateChangeEvent_is_propagated()
        {
            bool called = false;
            context.STATE_CHANGE += delegate ()
            {
                called = true;
            };
            context.Initialize();
            Assert.That(called, Is.True);
        }

        [Test]
        public void release_frees_up_the_instance()
        {
            object expected = new object();
            object actual = null;
            context.Released += delegate (object obj)
            {
                actual = obj;
            };
            context.Detain(expected);
            context.Release(expected);

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void removeChild_logs_warning_if_child_is_NOT_a_child()
        {
            LogParams? warning = null;
            context.AddLogTarget(new CallbackLogTarget(
                delegate (LogParams log)
                {
                    if (log.level == LogLevel.WARN)
                        warning = log;
                }));
            Context child = new Context();
            context.RemoveChild(child);

            Assert.That(warning, Is.Not.Null);
            Assert.That(warning.Value.message, Does.Contain("must be a child"));
            Assert.That(warning.Value.messageParameters, Is.EqualTo(new object[] { child, context }).AsCollection);
        }

        [Test]
        public void removed_child_is_not_removed_again_when_destroyed()
        {
            LogParams? warning = null;
            context.AddLogTarget(new CallbackLogTarget(
                delegate (LogParams log)
                {
                    if (log.level == LogLevel.WARN)
                        warning = log;
                }));
            Context child = new Context();
            context.AddChild(child);
            child.Initialize();
            context.RemoveChild(child);
            child.Destroy();
            Assert.That(warning, Is.Null);
        }

        [Test]
        public void removesChild_clears_child_parentInjector()
        {
            Context child = new Context();
            context.AddChild(child);
            context.RemoveChild(child);
            Assert.That(child.injector.parent, Is.Null);
        }

        [Test]
        public void resume_callback_is_fired()
        {
            context.Initialize();
            context.Suspend();
            context.Resume(delegate
            {
                Assert.Pass();
            });
            Assert.Fail();
        }

        [Test]
        public void suspend_callback_is_fired()
        {
            context.Initialize();
            context.Suspend(delegate
            {
                Assert.Pass();
            });
            Assert.Fail();
        }

        /*============================================================================*/
        /* Private Functions                                                          */
        /*============================================================================*/

        private Action<object> CreateValuePusher(List<object> list, object value)
        {
            return delegate (object context)
            {
                list.Add(value);
            };
        }

        private void nop()
        {
        }

        #endregion Methods
    }
}
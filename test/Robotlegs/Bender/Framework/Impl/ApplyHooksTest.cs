//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl.HookSupport;
using System.Collections.Generic;

namespace Robotlegs.Bender.Framework.Impl
{
	public class ApplyHooksTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector injector;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			injector = new RobotlegsInjector();
		}

		[TearDown]
		public void after()
		{
			injector = null;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void function_hooks_run()
		{
			int callCount = 0;
			Hooks.Apply (delegate () {
				callCount++;
			});
			Assert.AreEqual(callCount, 1);
		}

		[Test]
		public void class_hooks_run()
		{
			int callCount = 0;
			injector.Map (typeof(Action), "hookCallback").ToValue ((Action)delegate() {
				callCount++;
			});
			Hooks.Apply(injector, typeof(CallbackHook));
			Assert.AreEqual(callCount, 1);
		}

		[Test]
		public void instance_hooks_run()
		{
			int callCount = 0;
			CallbackHook hook = new CallbackHook (delegate() {
				callCount++;
			});
			Hooks.Apply(hook);
			Assert.AreEqual(callCount, 1);
		}

		[Test, ExpectedException]
		public void instance_without_hook_throws_error()
		{
			object invalidHook = new object();
			Hooks.Apply(invalidHook);
			// note: no assertion. we just want to know if an exception is thrown
		}

		[Test]
		public void instance_hooks_run_action()
		{
			int callCount = 0;
			Action hook = (Action)delegate() {
				callCount++;
			};
			Hooks.Apply(hook);
			Assert.AreEqual(callCount, 1);
		}

		[Test]
		public void instance_hooks_run_action_list()
		{
			int callCount = 0;
			Action hook = (Action)delegate() {
				callCount++;
			};
			Hooks.Apply(null, new List<Action>{hook, hook});
			Assert.AreEqual(callCount, 2);
		}
	}
}


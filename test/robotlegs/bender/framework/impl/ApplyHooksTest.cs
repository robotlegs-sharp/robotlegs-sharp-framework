using System;
using NUnit.Framework;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl.hookSupport;

namespace robotlegs.bender.framework.impl
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
			Hooks.Apply (new object[] {
				(Action)delegate () {
					callCount++;
				}
			});
			Assert.AreEqual(callCount, 1);
		}

		public void TestMethod()
		{
			Console.WriteLine ("Test");
		}

		[Test]
		public void class_hooks_run()
		{
			int callCount = 0;
			injector.Map (typeof(Action), "hookCallback").ToValue ((Action)delegate() {
				callCount++;
			});
			Hooks.Apply(new object[]{typeof(CallbackHook)}, injector);
			Assert.AreEqual(callCount, 1);
		}

		[Test]
		public void instance_hooks_run()
		{
			int callCount = 0;
			CallbackHook hook = new CallbackHook (delegate() {
				callCount++;
			});
			Hooks.Apply(new object[]{hook});
			Assert.AreEqual(callCount, 1);
		}

		[Test, ExpectedException]
		public void instance_without_hook_throws_error()
		{
			object invalidHook = new object();
			Hooks.Apply(new object[]{invalidHook});
			// note: no assertion. we just want to know if an exception is thrown
		}
	}
}


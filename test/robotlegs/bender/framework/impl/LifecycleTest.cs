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

//		private var target:Object;

		private Lifecycle lifecycle;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
//			target = {};
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

		[Test]
		public void initialize_turns_state_active()
		{
			lifecycle.Initialize();
			Assert.AreEqual (LifecycleState.ACTIVE, lifecycle.state);
			Assert.True (lifecycle.Active);
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
	}
}


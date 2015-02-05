using System;
using NUnit.Framework;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	[TestFixture]
	public class RobotlegsInjectorTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private RobotlegsInjector injector;

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
		public void parent_get_set()
		{
			IInjector expected = new RobotlegsInjector();
			injector.parent = expected;
			Assert.That(injector.parent, Is.EqualTo(expected));
		}

		[Test]
		public void createChild_remembers_parent()
		{
			IInjector child = injector.CreateChild();
			Assert.That(child.parent, Is.EqualTo(injector));
		}
	}
}


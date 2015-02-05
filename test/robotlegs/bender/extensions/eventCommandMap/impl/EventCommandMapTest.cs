using System;
using NUnit.Framework;
using Moq;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.eventCommandMap.impl
{
	[TestFixture]
	public class EventCommandTriggerTest
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<IEventDispatcher> dispatcher;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private EventCommandTrigger subject;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			dispatcher = new Mock<IEventDispatcher> ();
			subject = new EventCommandTrigger(new RobotlegsInjector(), dispatcher.Object, null, null);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void activating_adds_listener()
		{
			subject.Activate();
			dispatcher.Verify(target => target.AddEventListener(It.IsAny<Enum>(), It.IsAny<Action<IEvent>>()), Times.Once);
		}

		[Test]
		public void deactivating_removes_listener()
		{
			subject.Deactivate();
			dispatcher.Verify(target => target.RemoveEventListener(It.IsAny<Enum>(), It.IsAny<Action<IEvent>>()), Times.Once);
		}

	}
}


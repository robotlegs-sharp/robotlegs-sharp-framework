//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Moq;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Extensions.EventManagement.API;

namespace Robotlegs.Bender.Extensions.EventCommand.Impl
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


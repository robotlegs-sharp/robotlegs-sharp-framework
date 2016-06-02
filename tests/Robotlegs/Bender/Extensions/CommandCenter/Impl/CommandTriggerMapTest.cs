//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Moq;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.CommandCenter.Support;

namespace Robotlegs.Bender.Extensions.CommandCenter.Impl
{
	public class CommandTriggerMapTest
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<CommandMapStub> host;

		public Mock<ICommandTrigger> trigger;

		public Mock<ICommandExecutor> executor;

		public Mock<ICommandMappingList> mappings;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private CommandMapStub stubby;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			stubby = new CommandMapStub();
			host = new Mock<CommandMapStub> (MockBehavior.Strict);
			trigger = new Mock<ICommandTrigger> ();
			mappings = new Mock<ICommandMappingList> ();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void keyFactory_is_called_with_params()
		{
			object[] args = new object[]{ "hi", 5 };
			host.Setup (h => h.KeyFactory (args)).Returns ("anyKey");
			new CommandTriggerMap (host.Object.KeyFactory, stubby.TriggerFactory).GetTrigger (args);
			host.Verify (h => h.KeyFactory (args), Times.Once);
		}

		[Test]
		public void triggerFactory_is_called_with_with_params()
		{
			host.Setup (h => h.TriggerFactory ("hi", 5)).Returns (trigger.Object);
			new CommandTriggerMap (stubby.KeyFactory, host.Object.TriggerFactory).GetTrigger ("hi", 5);
			host.Verify (h => h.TriggerFactory ("hi", 5), Times.Once);
		}

		[Test]
		public void trigger_is_cached_by_key()
		{
			CommandTriggerMap subject = new CommandTriggerMap(stubby.KeyFactory, stubby.TriggerFactory);
			object mapper1 = subject.GetTrigger("hi", 5);
			object mapper2 = subject.GetTrigger("hi", 5);
			Assert.That(mapper1, Is.Not.Null);
			Assert.That(mapper1, Is.EqualTo(mapper2));
		}

		[Test]
		public void removeTrigger_deactivates_trigger()
		{
			host.Setup (h => h.TriggerFactory (It.IsAny<object[]>())).Returns (trigger.Object);
			trigger.Setup (t => t.Deactivate ());

			CommandTriggerMap subject = new CommandTriggerMap (stubby.KeyFactory, host.Object.TriggerFactory);
			subject.GetTrigger("hi", 5);
			subject.RemoveTrigger("hi", 5);
			trigger.Verify (t => t.Deactivate (), Times.Once);
		}
	}
}


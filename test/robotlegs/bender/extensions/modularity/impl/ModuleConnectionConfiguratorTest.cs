//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.extensions.eventDispatcher.api;
using Moq;
using System;
using NUnit.Framework;
using robotlegs.bender.extensions.eventCommandMap.support;

namespace robotlegs.bender.extensions.modularity.impl
{

	public class ModuleConnectionConfiguratorTest
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<IEventDispatcher> channelDispatcher = new Mock<IEventDispatcher>();

		public Mock<IEventDispatcher> localDispatcher = new Mock<IEventDispatcher>();

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private ModuleConnectionConfigurator subject;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			subject = new ModuleConnectionConfigurator(localDispatcher.Object, channelDispatcher.Object);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Sets_Up_Relaying_From_Local_To_Channel()
		{
			Enum expectedType = SupportEvent.Type.TYPE1;
			subject.RelayEvent(expectedType);
			localDispatcher.Verify (
				_localDispatcher => _localDispatcher.AddEventListener(
					It.Is<Enum>(arg1 => arg1 == expectedType),
					It.Is<Action<IEvent>>( arg2 => arg2 == channelDispatcher.Object.Dispatch)
				),
				Times.Once);
		}

		[Test]
		public void Sets_Up_Receiving_From_Channel_To_Local()
		{
			Enum expectedType = SupportEvent.Type.TYPE1;
			subject.ReceiveEvent(expectedType);
			channelDispatcher.Verify (
				_channelDispatcher => _channelDispatcher.AddEventListener(
					It.Is<Enum> (arg1 => arg1 == expectedType),
					It.Is<Action<IEvent>>(arg2 => arg2 == localDispatcher.Object.Dispatch)
				),
				Times.Once);
		}

		[Test]
		public void Suspends_Relaying()
		{
			Enum expectedType = SupportEvent.Type.TYPE1;
			subject.RelayEvent(expectedType);
			subject.Suspend();

			localDispatcher.Verify (
				_localDispatcher => _localDispatcher.RemoveEventListener(
					It.Is<Enum> (arg1 => arg1 == expectedType),
					It.IsAny<Action<IEvent>>()
				),
				Times.Once);
		}

		[Test]
		public void Resumes_Relaying()
		{
			Enum expectedType = SupportEvent.Type.TYPE1;
			subject.RelayEvent(expectedType);
			subject.Suspend();
			subject.Resume();

			localDispatcher.Verify (
				_localDispatcher => _localDispatcher.AddEventListener(
					It.Is<Enum> (arg1 => arg1 == expectedType),
					It.Is<Action<IEvent>>(arg1 => arg1 == channelDispatcher.Object.Dispatch)
				),
				Times.Exactly(2));
		}

		[Test]
		public void Suspends_Receiving()
		{
			Enum expectedType = SupportEvent.Type.TYPE1;
			subject.ReceiveEvent(expectedType);
			subject.Suspend();

			channelDispatcher.Verify (
				_channelDispatcher => _channelDispatcher.RemoveEventListener(
					It.Is<Enum> (arg1 => arg1 == expectedType),
					It.IsAny<Action<IEvent>>()
				),
				Times.Once);
		}

		[Test]
		public void Resumes_Receiving()
		{
			Enum expectedType = SupportEvent.Type.TYPE1;
			subject.ReceiveEvent(expectedType);
			subject.Suspend();
			subject.Resume();

			channelDispatcher.Verify (
				_channelDispatcher => _channelDispatcher.AddEventListener(
					It.Is<Enum> (arg1 => arg1 == expectedType),
					It.Is<Action<IEvent>>(arg2 => arg2 == localDispatcher.Object.Dispatch)
				),
				Times.Exactly(2));
		}

		[Test]
		public void Removes_Listeners_After_Destruction()
		{
			Enum expectedType1 = SupportEvent.Type.TYPE1;
			Enum expectedType2 = SupportEvent.Type.TYPE2;

			subject.RelayEvent(expectedType1);
			subject.ReceiveEvent(expectedType2);
			subject.Destroy();

			localDispatcher.Verify (
				_localDispatcher => _localDispatcher.RemoveEventListener(
					It.Is<Enum> (arg1 => arg1 == expectedType1),
					It.IsAny<Action<IEvent>>()
				),
				Times.Once);

			channelDispatcher.Verify (
				_channelDispatcher => _channelDispatcher.RemoveEventListener(
					It.Is<Enum> (arg1 => arg1 == expectedType2),
					It.IsAny<Action<IEvent>>()
				),
				Times.Once);
		}
	}
}

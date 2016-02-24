//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using System;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.EventManagement.API;

namespace Robotlegs.Bender.Extensions.LocalEventMap.Impl
{
	public class EventMapConfigTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private enum ConfigType
		{
			EXAMPLE,
			EXAMPLE_FALSE
		}

		private IEventDispatcher DISPATCHER = new EventDispatcher();

		private ConfigType EVENT_TYPE = ConfigType.EXAMPLE;

		private Action LISTENER = delegate{};

		private Type EVENT_CLASS = typeof(Event);

		private Action CALLBACK = delegate{};

		private EventMapConfig instance;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void setUp()
		{
			instance = new EventMapConfig(
				DISPATCHER,
				EVENT_TYPE,
				LISTENER,
				EVENT_CLASS,
				CALLBACK);
		}

		[TearDown]
		public void tearDown()
		{
			instance = null;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void can_be_instantiated()
		{
			Assert.That (instance, Is.InstanceOf<EventMapConfig> ());
		}

		[Test]
		public void get_dispatcher()
		{
			Assert.That (instance.dispatcher, Is.EqualTo(DISPATCHER)); 
		}

		[Test]
		public void get_eventString()
		{
			Assert.That(instance.type, Is.EqualTo(EVENT_TYPE));
		}

		[Test]
		public void get_listener()
		{
			Assert.That(instance.listener, Is.EqualTo(LISTENER));
		}

		[Test]
		public void get_eventClass()
		{
			Assert.That(instance.eventClass, Is.EqualTo(EVENT_CLASS));
		}

		[Test]
		public void get_callback()
		{
			Assert.That(instance.callback, Is.EqualTo(CALLBACK));
		}

		[Test]
		public void test_equality()
		{
			Assert.That(instance.Equals(DISPATCHER, EVENT_TYPE, LISTENER, EVENT_CLASS), Is.True);
		}

		[Test]
		public void test_equality_false()
		{
			Assert.That(instance.Equals(DISPATCHER, ConfigType.EXAMPLE_FALSE, LISTENER, EVENT_CLASS), Is.False);
		}
	}
}

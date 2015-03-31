//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.eventDispatcher;
using robotlegs.bender.extensions.eventCommandMap.api;

namespace robotlegs.bender.extensions.eventCommandMap
{
	[TestFixture]
	public class EventCommandMapExtensionTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Context context;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			context = new Context();
			context.Install<EventDispatcherExtension>();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void eventCommandMap_is_mapped_into_injector()
		{
			object actual = null;
			context.Install<EventCommandMapExtension>();
			context.WhenInitializing( (Action)delegate() {
				actual = context.injector.GetInstance(typeof(IEventCommandMap));
			});
			context.Initialize();
			Assert.That(actual, Is.InstanceOf<IEventCommandMap>());
		}
	}
}


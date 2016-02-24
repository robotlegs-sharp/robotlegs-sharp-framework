//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Framework.Impl
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


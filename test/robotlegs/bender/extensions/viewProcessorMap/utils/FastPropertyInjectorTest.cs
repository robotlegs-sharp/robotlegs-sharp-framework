//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Framework.Impl;
using NUnit.Framework;
using Robotlegs.Bender.Framework.API;
using System.Collections.Generic;
using System;

namespace Robotlegs.Bender.Extensions.ViewProcessor.Utils
{
	[TestFixture]
	public class FastPropertyInjectorTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private FastPropertyInjector instance;

		private IInjector injector;

		private int INT_VALUE = 3;

		private string STRING_VALUE = "someValue";

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			Dictionary<string,Type> config = new Dictionary<string, Type> ();
			config.Add ("intValue", typeof(int));
			config.Add ("stringValue", typeof(string));
			instance = new FastPropertyInjector(config);
			injector = new RobotlegsInjector();
		}

		[TearDown]
		public void tearDown()
		{
			instance = null;
			injector = null;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Can_Be_Instantiated()
		{
			Assert.That(instance, Is.InstanceOf(typeof(FastPropertyInjector)), "instance is FastPropertyInjector");
		}

		[Test]
		public void Process_Properties_Are_Injected()
		{
			injector.Map(typeof(int)).ToValue(INT_VALUE);
			injector.Map(typeof(string)).ToValue(STRING_VALUE);

			ViewToBeInjected view = new ViewToBeInjected();
			instance.Process(view, typeof(ViewToBeInjected), injector);

			Assert.That (view.intValue, Is.EqualTo (INT_VALUE));
			Assert.That (view.stringValue, Is.EqualTo (STRING_VALUE));
		}
	}
}
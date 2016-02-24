//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using Robotlegs.Bender.Framework.Impl;
using NUnit.Framework;
using Robotlegs.Bender.Framework.API;
using System;

namespace Robotlegs.Bender.Extensions.EnhancedLogging
{
	[TestFixture]
	public class InjectableLoggerExtensionTest
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
			context.Install<InjectableLoggerExtension>();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void logger_is_mapped_into_injector()
		{
			object actual = null;
			context.WhenInitializing((Action) delegate() {
				actual = context.injector.GetInstance(typeof(ILogging));
			});
			context.Initialize();
			Assert.That(actual, Is.InstanceOf<ILogging>());
		}
	}
}

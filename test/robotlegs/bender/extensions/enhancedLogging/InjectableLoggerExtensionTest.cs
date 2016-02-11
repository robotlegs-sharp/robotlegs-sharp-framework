//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//  Copyright (c) 2011 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using robotlegs.bender.framework.impl;
using NUnit.Framework;
using robotlegs.bender.framework.api;
using System;

namespace robotlegs.bender.extensions.enhancedLogging
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

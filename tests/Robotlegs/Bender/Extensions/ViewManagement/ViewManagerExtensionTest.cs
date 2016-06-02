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
using Robotlegs.Bender.Extensions.ViewManagement.API;

namespace Robotlegs.Bender.Extensions.ViewManagement
{
	[TestFixture]
	public class ViewManagerExtensionTest
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
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void installing_after_initialization_throws_error()
		{
            Assert.Throws(typeof(LifecycleException), new TestDelegate(() =>
            {
                context.Initialize();
                context.Install<ViewManagerExtension>();
            }
            ));
		}

		[Test]
		public void viewManager_is_mapped_into_injector()
		{
			object actual = null;
			context.Install<ViewManagerExtension>();
			context.WhenInitializing((Action)delegate() {
				actual = context.injector.GetInstance(typeof(IViewManager));
			});
			context.Initialize();
			Assert.That(actual, Is.InstanceOf<IViewManager>());
		}
	}
}

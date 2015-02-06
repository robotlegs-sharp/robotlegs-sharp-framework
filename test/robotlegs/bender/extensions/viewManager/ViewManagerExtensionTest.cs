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
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.extensions.viewManager
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

		[Test, ExpectedException(typeof(LifecycleException))]
		public void installing_after_initialization_throws_error()
		{
			context.Initialize();
			context.Install<ViewManagerExtension>();
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

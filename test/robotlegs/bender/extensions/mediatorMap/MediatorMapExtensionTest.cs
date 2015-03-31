//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using robotlegs.bender.framework.impl;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.mediatorMap.api;

namespace robotlegs.bender.extensions.mediatorMap
{
	public class MediatorMapExtensionTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Context context;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			context = new Context();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test, ExpectedException(typeof(LifecycleException))]
		public void Installing_After_Initialization_Throws_Error()
		{
			context.Initialize();
			context.Install(typeof(MediatorMapExtension));
		}

		[Test]
		public void MediatorMap_Is_Mapped_Into_Injector_On_Initialize()
		{
			object actual = null;
			context
				.Install(typeof(ViewManagerExtension))
					.Install(typeof(MediatorMapExtension));
			context.WhenInitializing(delegate() {
				actual = context.injector.GetInstance(typeof(IMediatorMap));
			});
			context.Initialize();
			Assert.That(actual, Is.InstanceOf(typeof(IMediatorMap)));
		}

		[Test]
		public void MediatorMap_Is_Unmapped_From_Injector_On_Destroy()
		{
			context
				.Install(typeof(ViewManagerExtension))
				.Install(typeof(MediatorMapExtension));
			context.AfterDestroying( delegate() {
				Assert.That(context.injector.HasMapping(typeof(IMediatorMap)), Is.False);
			});
			context.Initialize();
			context.Destroy();
		}

	}
}


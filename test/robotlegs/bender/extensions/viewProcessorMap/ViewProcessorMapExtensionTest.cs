//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.EventManagement.API;
using System.Collections.Generic;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Extensions.ViewManagement;
using Robotlegs.Bender.Extensions.ViewProcessor.API;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.ViewProcessor
{
	[TestFixture]
	public class ViewProcessorMapExtensionTest
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
			context.Install(typeof(ViewProcessorMapExtension));
		}

		[Test]
		public void ViewProcessorMap_Is_Mapped_Into_Injector_On_Initialize()
		{
			Object actual = null;
			context.Install (typeof(ViewManagerExtension));
			context.Install (typeof(ViewProcessorMapExtension));
			context.WhenInitializing( delegate() {
				actual = context.injector.GetInstance(typeof(IViewProcessorMap));
			});
			context.Initialize();
			Assert.That(actual, Is.InstanceOf(typeof(IViewProcessorMap)));
		}


		[Test]
		public void ViewProcessorMap_Is_Unmapped_From_Injector_On_Destroy()
		{
			context.Install (typeof(ViewManagerExtension));
			context.Install (typeof(ViewProcessorMapExtension));
			context.AfterDestroying( delegate {
				Assert.That(context.injector.SatisfiesDirectly(typeof(IViewProcessorMap)), Is.False);
			});
			context.Initialize();
			context.Destroy();
		}
	}
}

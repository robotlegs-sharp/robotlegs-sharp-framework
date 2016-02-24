//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Extensions.DirectCommand.API;

namespace Robotlegs.Bender.Extensions.DirectCommand
{
	[TestFixture]
	public class DirectCommandMapExtensionTest
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
			context.Install<DirectCommandMapExtension>();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void directCommandMap_is_mapped_into_injector()
		{
			object actual = null;
			context.WhenInitializing( delegate() {
				actual = context.injector.GetInstance<IDirectCommandMap>();
			});
			context.Initialize();
			Assert.That(actual, Is.InstanceOf<IDirectCommandMap>());
		}
	}
}


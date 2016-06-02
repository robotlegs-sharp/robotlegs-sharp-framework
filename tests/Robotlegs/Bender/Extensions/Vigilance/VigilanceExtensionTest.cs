//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;
using swiftsuspenders.errors;

namespace Robotlegs.Bender.Extensions.Vigilance
{
	[TestFixture]
	public class VigilanceExtensionTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private ILogging logger;

		private IInjector injector;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			IContext context = new Context ().Install<VigilanceExtension>();
			logger = context.GetLogger(this);
			injector = context.injector;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void extension_does_NOT_throw_for_DEBUG()
		{
			logger.Debug("");
		}

		[Test]
		public void extension_does_NOT_throw_for_INFO()
		{
			logger.Info("");
		}

		[Test]
		public void extension_throws_for_WARNING()
		{
            Assert.Throws(typeof(VigilantException), new TestDelegate(() =>
            {
                logger.Warn("");
            }
            ));
		}

		[Test]
		public void extension_throws_for_ERROR()
		{
            Assert.Throws(typeof(VigilantException), new TestDelegate(() =>
            {
                logger.Error("");
            }
            ));
		}

		[Test]
		public void extension_throws_for_FATAL()
		{
            Assert.Throws(typeof(VigilantException), new TestDelegate(() =>
            {
                logger.Fatal("");
            }
            ));
		}

		[Test]
		public void extension_throws_for_injector_MAPPING_override()
		{
            Assert.Throws(typeof(InjectorException), new TestDelegate(() =>
            {
                injector.Map<String>().ToValue("string");
                injector.Map<String>().ToValue("string");
            }
            ));
		}
	}
}


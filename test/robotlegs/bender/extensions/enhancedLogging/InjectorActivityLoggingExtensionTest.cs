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
using NUnit.Framework;
using robotlegs.bender.framework.impl;
using System;
using robotlegs.bender.extensions.enhancedLogging.support;
using robotlegs.bender.extensions.viewManager.support;
using System.Collections.Generic;
using robotlegs.bender.framework.impl.loggingSupport;

namespace robotlegs.bender.extensions.enhancedLogging
{
	[TestFixture]
	public class InjectorActivityLoggingExtensionTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private object supportContainer;

		private Context context;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			context = new Context();
			context.LogLevel = LogLevel.DEBUG;
			context.Install<InjectorActivityLoggingExtension>();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void POST_INSTANTIATE_is_logged()
		{
			LogParams logMessage = log_that_contains_message ("POST_INSTANTIATE");
			Assert.That (logMessage.messageParameters, Is.EqualTo (new List<object> {
				supportContainer,
				typeof(SupportContainer)
			}).AsCollection);
		}

		[Test]
		public void PRE_CONSTRUCT_is_logged()
		{
			LogParams logMessage = log_that_contains_message ("PRE_CONSTRUCT");
			Assert.That (logMessage.messageParameters, Is.EqualTo (new List<object> {
				supportContainer,
				typeof(SupportContainer)
			}).AsCollection);
		}

		[Test]
		public void POST_CONSTRUCT_is_logged()
		{
			LogParams logMessage = log_that_contains_message ("POST_CONSTRUCT");
			Assert.That (logMessage.messageParameters, Is.EqualTo (new List<object> {
				supportContainer,
				typeof(SupportContainer)
			}).AsCollection);
		}

		[Test]
		public void PRE_MAPPING_CREATE_is_logged()
		{
			LogParams logMessage = log_that_contains_message ("PRE_MAPPING_CREATE");
			Assert.That (logMessage.messageParameters, Is.EqualTo (new List<object> {
				typeof(SupportContainer), null
			}).AsCollection);
		}

		[Test]
		public void no_logging_after_context_is_destroyed()
		{
			context.Initialize();
			context.Destroy();
			Assert.That(getLog(), Is.Empty);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private LogParams log_that_contains_message(string messageContains)
		{
			List<LogParams> log = getLog ();
			foreach (LogParams lp in log)
			{
				Console.WriteLine (lp.message);
				if (lp.message.ToString().Contains(messageContains))
				{
					return lp;
				}
			}
			throw new Exception ("Cannot find log that contains message: " + messageContains);
		}

		private List<LogParams> getLog()
		{
			List<LogParams> loggingParams = new List<LogParams>();
			SupportLogTarget target = new SupportLogTarget((Action<LogParams>)delegate(LogParams arg) {
				loggingParams.Add(arg);
			});
			context.AddLogTarget(target);
			context.injector.Map(typeof(SupportContainer));
			supportContainer = context.injector.GetInstance(typeof(SupportContainer));
			return loggingParams;
		}
	}
}
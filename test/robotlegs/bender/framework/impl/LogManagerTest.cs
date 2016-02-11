//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using NUnit.Framework;
using robotlegs.bender.framework.impl.loggingSupport;
using System.Collections.Generic;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	[TestFixture]
	public class LogManagerTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private object source;

		private LogManager logManager;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			source = new object();
			logManager = new LogManager();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void level_is_set()
		{
			logManager.logLevel = LogLevel.WARN;
			Assert.AreEqual(logManager.logLevel, LogLevel.WARN);
		}

		[Test]
		public void get_logger()
		{
			Assert.IsInstanceOf<ILogging>(logManager.GetLogger(source));
		}

		[Test]
		public void added_targets_are_logged_to()
		{
			string[] expected = new string[]{"target1", "target2", "target3"};
			List<string> actual = new List<string> ();
			logManager.AddLogTarget(new CallbackLogTarget(delegate (LogParams result) {
				actual.Add("target1");
			}));
			logManager.AddLogTarget(new CallbackLogTarget(delegate (LogParams result) {
				actual.Add("target2");
			}));
			logManager.AddLogTarget(new CallbackLogTarget(delegate (LogParams result) {
				actual.Add("target3");
			}));
			logManager.GetLogger(source).Info(expected);
			Assert.That (actual, Is.EqualTo (expected).AsCollection);
		}
	}
}
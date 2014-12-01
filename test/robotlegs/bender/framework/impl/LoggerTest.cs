using System;
using NUnit.Framework;
using robotlegs.bender.framework.impl.loggingSupport;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
{
	[TestFixture]
	public class LoggerTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private object source;

		private Logger logger;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Before()
		{
			Context c = new Context ();
			source = new object();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void source_is_passed()
		{
			object expected = source;
			object actual = null;
			logger = new Logger (source, new CallbackLogTarget (delegate (LogParams result) {
				actual = result.source;
			}));
			logger.Debug("hello");
			Assert.AreEqual (actual, expected);
		}

		[Test]
		public void level_is_passed()
		{
			LogLevel[] expected = new LogLevel[]{LogLevel.FATAL, LogLevel.ERROR, LogLevel.WARN, LogLevel.INFO, LogLevel.DEBUG};
			List<LogLevel> actual = new List<LogLevel>();
			logger = new Logger(source, new CallbackLogTarget(delegate (LogParams result) {
				actual.Add(result.level);
			}));
			logger.Fatal("fatal");
			logger.Error("error");
			logger.Warn("warn");
			logger.Info("info");
			logger.Debug("debug");
			Assert.That (actual.ToArray(), Is.EqualTo (expected).AsCollection );
		}

		[Test]
		public void message_is_passed()
		{
			object expected = "hello";
			object actual = null;
			logger = new Logger(source, new CallbackLogTarget(delegate (LogParams result) {
				actual = result.message;
			}));
			logger.Debug(expected);
			Assert.AreEqual(actual, expected);
		}

		[Test]
		public void params_are_passed()
		{
			object[] expected = new object[]{1, 2, 3};
			object[] actual = null;
			logger = new Logger(source, new CallbackLogTarget(delegate (LogParams result) {
				actual = result.messageParameters;
			}));
			logger.Debug("hello", expected);
			Assert.That(actual, Is.EqualTo(expected).AsCollection );
		}
	}
}
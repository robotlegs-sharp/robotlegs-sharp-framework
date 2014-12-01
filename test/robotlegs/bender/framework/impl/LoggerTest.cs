using System;
using NUnit.Framework;

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
//			logger = new Logger(source, new CallbackLogTarget
//			logger = new Logger(source, new CallbackLogTarget(function(result:Object):void {
//				actual = result.source;
//			}));
//			logger.Debug("hello");
			Assert.AreEqual (actual, expected);
		}

		/*
		[Test]
		public function level_is_passed():void
		{
			const expected:Array = [LogLevel.FATAL, LogLevel.ERROR, LogLevel.WARN, LogLevel.INFO, LogLevel.DEBUG];
			var actual:Array = [];
			logger = new Logger(source, new CallbackLogTarget(function(result:Object):void {
				actual.push(result.level);
			}));
			logger.fatal("fatal");
			logger.error("error");
			logger.warn("warn");
			logger.info("info");
			logger.debug("debug");
			assertThat(actual, array(expected));
		}

		[Test]
		public function message_is_passed():void
		{
			const expected:String = "hello";
			var actual:String = null;
			logger = new Logger(source, new CallbackLogTarget(function(result:Object):void {
				actual = result.message;
			}));
			logger.debug(expected);
			assertThat(actual, equalTo(expected));
		}

		[Test]
		public function params_are_passed():void
		{
			const expected:Array = [1, 2, 3];
			var actual:Array = null;
			logger = new Logger(source, new CallbackLogTarget(function(result:Object):void {
				actual = result.params;
			}));
			logger.debug("hello", expected);
			assertThat(actual, equalTo(expected));
		}
		*/
	}
}
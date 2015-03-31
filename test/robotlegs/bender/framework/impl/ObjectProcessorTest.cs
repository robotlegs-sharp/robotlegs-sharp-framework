//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.framework.impl.safelyCallBackSupport;

namespace robotlegs.bender.framework.impl
{
	[TestFixture]
	public class ObjectProcessorTest
	{
		/*============================================================================*/
		/* Protected Properties                                                       */
		/*============================================================================*/

		protected ObjectProcessor objectProcessor;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			objectProcessor = new ObjectProcessor();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void addObjectHandler()
		{
			objectProcessor.AddObjectHandler(new InstanceOfMatcher (typeof(String)), (Action<object>)delegate(object obj){});
		}

		[Test]
		public void addObject()
		{
			objectProcessor.ProcessObject(new object());
		}

		[Test]
		public void handler_handles_object()
		{
			object expected = "string";
			object actual = null;
			objectProcessor.AddObjectHandler(new InstanceOfMatcher (typeof(String)), (Action<object>)delegate(object obj){
				actual = obj;
			});
			objectProcessor.ProcessObject(expected);
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void handler_does_not_handle_wrong_object()
		{
			object expected = "string";
			object actual = null;
			objectProcessor.AddObjectHandler(new InstanceOfMatcher (typeof(bool)), (Action<object>)delegate(object obj){
				actual = obj;
			});
			objectProcessor.ProcessObject(expected);
			Assert.That(actual, Is.Not.EqualTo(expected));
		}

		[Test]
		public void handlers_handle_object()
		{
			List<object> expected = new List<object>{ "handler1", "handler2", "handler3" };
			List<object> actual = new List<object> ();
			objectProcessor.AddObjectHandler(new InstanceOfMatcher (typeof(String)), CreateHandler.Handler<object>((Action<object>)actual.Add, new object[]{"handler1"}));
			objectProcessor.AddObjectHandler(new InstanceOfMatcher (typeof(String)), CreateHandler.Handler<object>((Action<object>)actual.Add, new object[]{"handler2"}));
			objectProcessor.AddObjectHandler(new InstanceOfMatcher (typeof(String)), CreateHandler.Handler<object>((Action<object>)actual.Add, new object[]{"handler3"}));

			objectProcessor.ProcessObject("string");
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void removeAllHandlers()
		{
			objectProcessor.AddObjectHandler(new InstanceOfMatcher (typeof(String)), (Action<object>)delegate(object obj){
				Assert.Fail("Handler should not fire after call to removeAllHandlers");
			});
			objectProcessor.RemoveAllHandlers();
			objectProcessor.ProcessObject("string");
		}
	}
}


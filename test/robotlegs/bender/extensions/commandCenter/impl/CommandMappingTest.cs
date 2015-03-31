//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using robotlegs.bender.extensions.commandCenter.support;

namespace robotlegs.bender.extensions.commandCenter.impl
{
	public class CommandMappingTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private CommandMapping mapping;

		private Type commandClass;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			commandClass = typeof(NullCommand);
			mapping = new CommandMapping(commandClass);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void mapping_stores_Command()
		{
			Assert.That(mapping.CommandClass, Is.EqualTo(commandClass));
		}

		[Test]
		public void default_ExecuteMethod()
		{
			Assert.That(mapping.ExecuteMethod, Is.EqualTo("Execute"));
		}

		[Test]
		public void mapping_stores_ExecuteMethod()
		{
			mapping.SetExecuteMethod("run");
			Assert.That(mapping.ExecuteMethod, Is.EqualTo("run"));
		}

		[Test]
		public void mapping_stores_Guards()
		{
			mapping.AddGuards(1, 2, 3);
			Assert.That(mapping.Guards, Is.EqualTo(new object[]{1,2,3}).AsCollection);
		}

		[Test]
		public void mapping_stores_GuardsArray()
		{
			mapping.AddGuards(new object[]{1, 2, 3});
			Assert.That(mapping.Guards, Is.EqualTo(new object[]{1,2,3}).AsCollection);
		}

		[Test]
		public void mapping_stores_Hooks()
		{
			mapping.AddHooks(1, 2, 3);
			Assert.That(mapping.Hooks, Is.EqualTo(new object[]{1,2,3}).AsCollection);
		}

		[Test]
		public void mapping_stores_HooksArray()
		{
			mapping.AddHooks(new object[]{1, 2, 3});
			Assert.That(mapping.Hooks, Is.EqualTo(new object[]{1,2,3}).AsCollection);
		}

		[Test]
		public void fireOnce_defaults_to_False()
		{
			Assert.That(mapping.FireOnce, Is.False);
		}

		[Test]
		public void mapping_stores_FireOnce()
		{
			mapping.SetFireOnce(true);
			Assert.That(mapping.FireOnce, Is.True);
		}

		[Test]
		public void mapping_stores_FireOnce_when_false()
		{
			mapping.SetFireOnce(false);
			Assert.That(mapping.FireOnce, Is.False);
		}

		[Test]
		public void payloadInjectionEnabled_defaults_to_True()
		{
			Assert.That(mapping.PayloadInjectionEnabled, Is.True);
		}

		[Test]
		public void mapping_stores_PayloadInjectionEnabled()
		{
			mapping.SetPayloadInjectionEnabled(false);
			Assert.That(mapping.PayloadInjectionEnabled, Is.False);
		}
	}
}


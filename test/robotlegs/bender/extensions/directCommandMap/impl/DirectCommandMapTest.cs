//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using robotlegs.bender.extensions.directCommandMap.api;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.commandCenter.support;
using robotlegs.bender.extensions.directCommandMap.support;
using robotlegs.bender.extensions.commandCenter.api;
using robotlegs.bender.extensions.commandCenter.impl;

namespace robotlegs.bender.extensions.directCommandMap.impl
{
	public class DirectCommandMapTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext context;

		private DirectCommandMap subject;

		private IInjector injector;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			context = new Context();
			injector = context.injector;
			injector.Map<IDirectCommandMap>().ToType<DirectCommandMap>();
			subject = injector.GetInstance<IDirectCommandMap>() as DirectCommandMap;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void map_creates_IOnceCommandConfig()
		{
			Assert.That(subject.Map<NullCommand>(), Is.InstanceOf<IDirectCommandConfigurator>());
		}

		[Test]
		public void successfully_executes_command_classes()
		{
			int executionCount = 0;
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate() {
				executionCount++;
			});

			subject.Map<CallbackCommand>()
				.Map<CallbackCommand2>()
				.Execute();

			Assert.That(executionCount, Is.EqualTo(2));
		}

		[Test]
		public void commands_get_injected_with_DirectCommandMap_instance()
		{
			IDirectCommandMap actual = null;
			injector.Map(typeof(Action<IDirectCommandMap>), "ReportingFunction").ToValue((Action<IDirectCommandMap>)delegate(IDirectCommandMap passed) {
				actual = passed;
			});

			subject.Map<DirectCommandMapReportingCommand>().Execute();

			Assert.That(actual, Is.EqualTo(subject));
		}

		[Test]
		public void commands_are_disposed_after_execution()
		{
			int executionCount = 0;
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate() {
				executionCount++;
			});

			subject.Map<CallbackCommand>().Execute();
			subject.Map<CallbackCommand>().Execute();

			Assert.That(executionCount, Is.EqualTo(2));
		}

		[Test]
		public void sandboxed_directCommandMap_instance_does_not_leak_into_system()
		{
			IDirectCommandMap actual = injector.GetInstance<IDirectCommandMap>();

			Assert.That(actual, Is.Not.EqualTo(subject));
		}

		[Test]
		public void detains_command()
		{
			object command = new object();
			bool wasDetained = false;
			context.Detained += delegate(object obj)
			{
				wasDetained = true;
			};
			subject.Detain(command);

			Assert.That(wasDetained, Is.True);
		}

		[Test]
		public void releases_command()
		{
			object command = new object();
			bool wasReleased = false;
			context.Released += delegate (object obj) {
				wasReleased = true;
			};
			subject.Detain(command);

			subject.Release(command);

			Assert.That(wasReleased, Is.True);
		}

		[Test]
		public void executes_command()
		{
			int executionCount = 0;
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate() {
				executionCount++;
			});

			subject.Map<CallbackCommand>();
			subject.Execute();

			Assert.That(executionCount, Is.EqualTo(1));
		}

		[Test]
		public void mapping_processor_is_called()
		{
			int callCount = 0;
			subject.AddMappingProcessor((CommandMappingList.Processor)delegate(ICommandMapping mapping) {
				callCount++;
			});
			subject.Map<NullCommand>();
			Assert.That(callCount, Is.EqualTo(1));
		}
	}
}


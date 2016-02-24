//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Framework.Impl;
using Moq;
using Robotlegs.Bender.Extensions.CommandCenter.Support;
using Robotlegs.Bender.Framework.Impl.GuardSupport;

namespace Robotlegs.Bender.Extensions.CommandCenter.Impl
{
	[TestFixture]
	public class CommandExecutorTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Mock<UnmapperStub> unMapper;

		private List<ICommandMapping> mappings;

		private CommandExecutor subject;

		private List<object> reported;

		private IInjector injector;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			unMapper = new Mock<UnmapperStub> ();
			reported = new List<object> ();
			injector = new RobotlegsInjector();

			injector.Map(typeof(Action<object>), "ReportingFunction").ToValue((Action<object>)reportingFunction);
			mappings = new List<ICommandMapping>();
			subject = new CommandExecutor(injector);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void oneShotMapping_is_removed()
		{
			subject = new CommandExecutor(injector, unMapper.Object.Unmap);
			ICommandMapping mapping = addMapping<ClassReportingCallbackCommand>();
			mapping.SetFireOnce(true);

			subject.ExecuteCommands(mappings);

			unMapper.Verify (unMapperObject => unMapperObject.Unmap (It.Is<ICommandMapping> (arg => arg == mapping)), Times.Once);
		}

		[Test]
		public void command_without_execute_method_is_still_constructed()
		{
			addMapping<ExecutelessCommand>().SetExecuteMethod(null);
			subject.ExecuteCommands(mappings);

			Assert.That(reported, Is.EqualTo(new List<object>{typeof(ExecutelessCommand)}).AsCollection);
		}

		[Test]
		public void command_is_executed()
		{
			addMapping();
			executeCommands();
			Assert.That(reported, Is.EqualTo(new List<object>{typeof(ClassReportingCallbackCommand)}));
		}

		[Test]
		public void command_is_executed_repeatedly()
		{
			addMappings(5);
			executeCommands();
			Assert.That(reported.Count, Is.EqualTo(5));
		}

		[Test]
		public void hooks_are_called()
		{
			addMapping<NullCommand>().AddHooks(
				typeof(ClassReportingCallbackHook), typeof(ClassReportingCallbackHook), typeof(ClassReportingCallbackHook));
			executeCommands();
			Assert.That(reported.Count, Is.EqualTo(3));
		}

		[Test]
		public void command_is_injected_into_hook()
		{
			SelfReportingCallbackCommand executedCommand = null;
			SelfReportingCallbackCommand injectedCommand = null;
			injector.Map(typeof(Action<SelfReportingCallbackCommand>), "ExecuteCallback").ToValue((Action<SelfReportingCallbackCommand>)delegate(SelfReportingCallbackCommand command){
				executedCommand = command;
			});
			injector.Map(typeof(Action<SelfReportingCallbackHook>), "HookCallback").ToValue((Action<SelfReportingCallbackHook>)delegate(SelfReportingCallbackHook hook){
				injectedCommand = hook.command;
			});
			addMapping<SelfReportingCallbackCommand>().AddHooks<SelfReportingCallbackHook>();
			executeCommands();
			Assert.That(injectedCommand, Is.EqualTo(executedCommand));
		}

		[Test]
		public void command_executes_when_the_guard_allows()
		{
			addMapping().AddGuards<HappyGuard>();
			executeCommands();
			Assert.That(reported, Is.EqualTo(new List<object>{typeof(ClassReportingCallbackCommand)}).AsCollection);
		}

		[Test]
		public void command_does_not_execute_when_any_guards_denies()
		{
			addMapping().AddGuards<HappyGuard, GrumpyGuard>();
			executeCommands();
			Assert.That(reported, Is.Empty);
		}

		[Test]
		public void execution_sequence_is_guard_command_guard_command_with_multiple_mappings()
		{
			addMapping<ClassReportingCallbackCommand>().AddGuards<ClassReportingCallbackGuard>();
			addMapping<ClassReportingCallbackCommand2>().AddGuards<ClassReportingCallbackGuard2>();
			executeCommands();
			Assert.That(reported, Is.EqualTo(new object[]{
				typeof(ClassReportingCallbackGuard), typeof(ClassReportingCallbackCommand),
				typeof(ClassReportingCallbackGuard2), typeof(ClassReportingCallbackCommand2)}).AsCollection);
		}

		[Test]
		public void execution_sequence_is_guard_hook_command()
		{
			addMapping().AddGuards<ClassReportingCallbackGuard>().AddHooks<ClassReportingCallbackHook>();
			executeCommands();
			Assert.That(reported, Is.EqualTo(new object[]{
				typeof(ClassReportingCallbackGuard), typeof(ClassReportingCallbackHook), typeof(ClassReportingCallbackCommand)}).AsCollection);
		}

		[Test]
		public void allowed_commands_get_executed_after_denied_command()
		{
			addMapping<ClassReportingCallbackCommand>().AddGuards<GrumpyGuard>();
			addMapping<ClassReportingCallbackCommand2>();
			executeCommands();
			Assert.That(reported, Is.EqualTo(new object[]{typeof(ClassReportingCallbackCommand2)}).AsCollection);
		}

		[Test]
		public void command_with_different_method_than_execute_is_called()
		{
			addMapping<ReportMethodCommand>()
				.SetExecuteMethod("Report");

			executeCommands();

			Assert.That(reported, Is.EqualTo(new object[]{typeof(ReportMethodCommand)}).AsCollection);
		}

		[Test, ExpectedException]
		public void throws_error_when_executeMethod_not_a_function()
		{
			addMapping<IncorrectExecuteCommand>();
			executeCommands();
			// note: no assertion. we just want to know if an error is thrown
		}

		[Test]
		public void payload_is_injected_into_command()
		{
			addMapping<PayloadInjectionPointsCommand>();
			CommandPayload payload = new CommandPayload(new List<object>{"message", 1}, new List<Type>{typeof(String), typeof(int)});
			executeCommands(payload);

			Assert.That(reported, Is.EqualTo(payload.Values).AsCollection);
		}

		[Test]
		public void payload_is_injected_into_hook()
		{
			addMapping<NullCommand>().AddHooks<PayloadInjectionPointsHook>();
			CommandPayload payload = new CommandPayload(new List<object>{"message", 1}, new List<Type>{typeof(String), typeof(int)});
			executeCommands(payload);
			Assert.That(reported, Is.EqualTo(payload.Values).AsCollection);
		}

		[Test]
		public void payload_is_injected_into_guard()
		{
			addMapping<NullCommand>().AddGuards<PayloadInjectionPointsGuard>();
			CommandPayload payload = new CommandPayload(new List<object>{"message", 1}, new List<Type>{typeof(String), typeof(int)});
			executeCommands(payload);
			Assert.That(reported, Is.EqualTo(payload.Values).AsCollection);
		}

		[Test]
		public void payload_is_passed_to_execute_method()
		{
			addMapping<MethodParametersCommand>();
			CommandPayload payload = new CommandPayload(new List<object>{"message", 1}, new List<Type>{typeof(String), typeof(int)});
			executeCommands(payload);
			Assert.That(reported, Is.EqualTo(payload.Values).AsCollection);
		}

		[Test]
		public void payloadInjection_is_disabled()
		{
			addMapping<OptionalInjectionPointsCommand>()
				.SetPayloadInjectionEnabled(false);

			CommandPayload payload = new CommandPayload(new List<object>{"message", 1}, new List<Type>{typeof(String), typeof(int)});
			executeCommands(payload);
			Assert.That(reported, Is.EqualTo(new object[]{null, 0}).AsCollection);
		}

		[Test]
		public void payload_doesnt_leak_into_class_instantiated_by_command()
		{
			injector.Map<IInjector>().ToValue(injector);
			addMapping<OptionalInjectionPointsCommandInstantiatingCommand>();

			CommandPayload payload = new CommandPayload(new List<object>{"message", 1}, new List<Type>{typeof(String), typeof(int)});
			executeCommands(payload);
			Assert.That(reported, Is.EqualTo(new object[]{null, 0}).AsCollection);
		}

		[Test]
		public void result_is_handled()
		{
			ICommandMapping mapping = new CommandMapping(typeof(MessageReturningCommand));
			subject = new CommandExecutor(injector, null, resultReporter);
			injector.Map<String>().ToValue("message");
			subject.ExecuteCommand(mapping);
			Assert.That (reported.Count, Is.EqualTo(1));
			Assert.That (reported [0], Is.InstanceOf<Dictionary<string, object>> ());
			Dictionary<string, object> reportedDict = reported [0] as Dictionary<string, object>;
			Assert.That (reportedDict["result"], Is.EqualTo("message"));
			Assert.That (reportedDict["command"], Is.InstanceOf<MessageReturningCommand>());
			Assert.That (reportedDict["mapping"], Is.EqualTo(mapping));
		}

		[Test]
		public void uses_injector_mapped_command_instance()
		{
			injector.Map(typeof(Action<SelfReportingCallbackCommand>), "ExecuteCallback").ToValue((Action<SelfReportingCallbackCommand>)reportingFunction);
			injector.Map<SelfReportingCallbackCommand>().AsSingleton();
			object expected = injector.GetInstance<SelfReportingCallbackCommand>();
			ICommandMapping mapping = addMapping(typeof(SelfReportingCallbackCommand));
			subject.ExecuteCommand(mapping);
			Assert.That(reported, Is.EqualTo(new object[]{expected}).AsCollection);
		}

		[Test]
		public void command_mapped_to_interface_is_executed()
		{
			injector.Map<ICommand> ().ToType (typeof(AbstractInterfaceImplementingCommand));
			subject.ExecuteCommand(addMapping<ICommand>());
			Assert.That(reported, Is.EqualTo(new object[]{typeof(AbstractInterfaceImplementingCommand)}).AsCollection);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private ICommandMapping addMapping<T>()
		{
			return addMapping (typeof(T));
		}

		private ICommandMapping addMapping(Type commandClass = null)
		{

			if (commandClass == null)
				commandClass = typeof(ClassReportingCallbackCommand);
			ICommandMapping mapping = new CommandMapping(commandClass);
			mappings.Add(mapping);
			return mapping;
		}

		private void addMappings(uint totalEvents = 1, Type commandClass = null)
		{
			while (totalEvents-- > 0)
			{
				addMapping(commandClass);
			}
		}

		private void executeCommands(CommandPayload payload = null)
		{
			subject.ExecuteCommands(mappings, payload);
		}

		private void reportingFunction(object item)
		{
			reported.Add(item);
		}

		private void resultReporter(object result, object command, ICommandMapping mapping)
		{
			Dictionary<string, object> dict = new Dictionary<string, object> ();
			dict.Add ("result", result);
			dict.Add ("command", command);
			dict.Add ("mapping", mapping);
			reported.Add(dict);
		}
	}
}


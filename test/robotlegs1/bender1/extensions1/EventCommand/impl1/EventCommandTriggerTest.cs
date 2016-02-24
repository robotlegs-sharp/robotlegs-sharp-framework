//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using System.Collections.Generic;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.CommandCenter.DSL;
using Robotlegs.Bender.Extensions.EventCommand.API;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Extensions.EventCommand.Support;
using Robotlegs.Bender.Extensions.CommandCenter.Support;
using Robotlegs.Bender.Framework.Impl.GuardSupport;
using Robotlegs.Bender.Extensions.CommandCenter.Impl;
using Robotlegs.Bender.Extensions.CommandCenter.API;

namespace Robotlegs.Bender.Extensions.EventCommand.Impl
{
	[TestFixture]
	public class EventCommandMapTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private enum TestEnum
		{
			TEST
		}

		private IEventCommandMap subject;

		private ICommandMapper mapper;

		private List<object> reportedExecutions;

		private IInjector injector;

		private IEventDispatcher dispatcher;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			reportedExecutions = new List<object>();
			IContext context = new Context();
			injector = context.injector;
			injector.Map(typeof(Action<object>), "ReportingFunction").ToValue((Action<object>)reportingFunction);
			dispatcher = new EventDispatcher();
			subject = new EventCommandMap(context, dispatcher);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void map_creates_mapper()
		{
			Assert.That(subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent)), Is.InstanceOf(typeof(ICommandMapper)));
		}

		[Test]
		public void map_to_identical_Type_but_different_Event_returns_different_mapper()
		{
			mapper = subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent));
			Assert.That (subject.Map (SupportEvent.Type.TYPE1, typeof(Event)), Is.Not.EqualTo (mapper));
		}

		[Test]
		public void map_to_different_Type_but_identical_Event_returns_different_mapper()
		{
			mapper = subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent));
			Assert.That (subject.Map (SupportEvent.Type.TYPE2, typeof(SupportEvent)), Is.Not.EqualTo (mapper));
		}

		[Test]
		public void unmap_returns_mapper()
		{
			mapper = subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent));
			Assert.That(subject.Unmap(SupportEvent.Type.TYPE1, typeof(SupportEvent)), Is.InstanceOf(typeof(ICommandUnmapper)));
		}

		[Test]
		public void robust_to_unmapping_non_existent_mappings()
		{
			subject.Unmap(SupportEvent.Type.TYPE1).FromCommand(typeof(NullCommand));
			// note: no assertion, just testing for the lack of an NPE
		}

		[Test]
		public void command_executes_successfully()
		{
			Assert.That(commandExecutionCount(1), Is.EqualTo(1));
		}

		[Test]
		public void command_executes_repeatedly()
		{
			Assert.That(commandExecutionCount(5), Is.EqualTo(5));
		}

		[Test]
		public void fireOnce_command_executes_once()
		{
			Assert.That(oneshotCommandExecutionCount(5), Is.EqualTo(1));
		}

		[Test] //TODO: Make this unit test work
		public void event_is_injected_into_command()
		{
			IEvent injectedEvent = null;
			injector.Map(typeof(Action<EventInjectedCallbackCommand>), "ExecuteCallback").ToValue((Action<EventInjectedCallbackCommand>)delegate(EventInjectedCallbackCommand command)
			{
				injectedEvent = command.evt;
			});
			subject.Map(SupportEvent.Type.TYPE1, typeof(IEvent))
				.ToCommand<EventInjectedCallbackCommand>();
			SupportEvent evt = new SupportEvent(SupportEvent.Type.TYPE1);
			dispatcher.Dispatch(evt);
			Assert.That(injectedEvent, Is.EqualTo(evt));
		}

		[Test]
		public void event_is_passed_to_execute_method()
		{
			SupportEvent actualEvent = null;
			injector.Map(typeof(Action<SupportEvent>), "ExecuteCallback").ToValue((Action<SupportEvent>)delegate(SupportEvent evt)
			{
				actualEvent = evt;
			});
			subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent))
				.ToCommand<EventParametersCommand>();
			SupportEvent supportEvent = new SupportEvent(SupportEvent.Type.TYPE1);
			dispatcher.Dispatch(supportEvent);
			Assert.That(actualEvent, Is.EqualTo(supportEvent));
		}

		[Test]
		public void concretely_specified_typed_event_is_injected_into_command_as_typed_event()
		{
			SupportEvent injectedEvent = null;
			injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback").ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command)
			{
				injectedEvent = command.typedEvent;
			});
			subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent))
				.ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
			SupportEvent supportEvent = new SupportEvent(SupportEvent.Type.TYPE1);
			dispatcher.Dispatch(supportEvent);
			Assert.That(injectedEvent, Is.EqualTo(supportEvent));
		}

		[Test] //TODO: Make this unit test work
		public void abstractly_specified_typed_event_is_injected_into_command_as_untyped_event()
		{
			IEvent injectedEvent = null;
			injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback").ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command)
			{
				injectedEvent = command.untypedEvent;
			});
			subject.Map(SupportEvent.Type.TYPE1, typeof(IEvent))
				.ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
			SupportEvent supportEvent = new SupportEvent(SupportEvent.Type.TYPE1);
			dispatcher.Dispatch(supportEvent);
			Assert.That(injectedEvent, Is.EqualTo(supportEvent));
		}

		[Test]
		public void unspecified_typed_event_is_injected_into_command_as_typed_event()
		{
			SupportEvent injectedEvent = null;
			injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback").ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command)
			{
				injectedEvent = command.typedEvent;
			});
			subject.Map(SupportEvent.Type.TYPE1)
				.ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
			SupportEvent supportEvent = new SupportEvent(SupportEvent.Type.TYPE1);
			dispatcher.Dispatch(supportEvent);
			Assert.That(injectedEvent, Is.EqualTo(supportEvent));
		}

		[Test] //TODO: Make this unit test work
		public void unspecified_untyped_event_is_injected_into_command_as_untyped_event()
		{

			Enum eventType = TestEnum.TEST;
			IEvent injectedEvent = null;
			injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback").ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command)
			{
				injectedEvent = command.untypedEvent;
			});
			subject.Map(eventType)
				.ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
			Event evt = new Event(eventType);
			dispatcher.Dispatch(evt);
			Assert.That(injectedEvent, Is.EqualTo(evt));
		}

		[Test] //TODO: Make this unit test work
		public void specified_untyped_event_is_injected_into_command_as_untyped_event()
		{
			Enum eventType = TestEnum.TEST;
			IEvent injectedEvent = null;
			injector.Map(typeof(Action<SupportEventTriggeredSelfReportingCallbackCommand>), "ExecuteCallback").ToValue((Action<SupportEventTriggeredSelfReportingCallbackCommand>)delegate(SupportEventTriggeredSelfReportingCallbackCommand command)
			{
				injectedEvent = command.untypedEvent;
			});
			subject.Map(eventType, typeof(IEvent))
				.ToCommand<SupportEventTriggeredSelfReportingCallbackCommand>();
			Event evt = new Event(eventType);
			dispatcher.Dispatch(evt);
			Assert.That(injectedEvent, Is.EqualTo(evt));
		}

		[Test]
		public void command_does_not_execute_when_incorrect_eventType_dispatched()
		{
			uint executeCount = 0;
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate()
			{
				executeCount++;
			});
			subject.Map(SupportEvent.Type.TYPE1).ToCommand<CallbackCommand>();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE2));
			Assert.That(executeCount, Is.EqualTo(0));
		}

		[Test] //TODO: Make this unit test work
		public void command_does_not_execute_when_incorrect_eventClass_dispatched()
		{
			uint executeCount = 0;
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate()
			{
				executeCount++;
			});
			subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent)).ToCommand<CallbackCommand>();
			dispatcher.Dispatch(new Event(SupportEvent.Type.TYPE1));
			Assert.That(executeCount, Is.EqualTo(0));
		}

		[Test]
		public void command_does_not_execute_after_event_unmapped()
		{
			uint executeCount = 0;
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate()
			{
				executeCount++;
			});
			subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent)).ToCommand<CallbackCommand>();
			subject.Unmap(SupportEvent.Type.TYPE1, typeof(SupportEvent)).FromCommand<CallbackCommand>();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			Assert.That(executeCount, Is.EqualTo(0));
		}

		[Test]
		public void oneshot_mappings_should_not_bork_stacked_mappings()
		{
			uint executeCount = 0;
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate()
			{
				executeCount++;
			});
			subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent)).ToCommand<CallbackCommand>().Once();
			subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent)).ToCommand<CallbackCommand2>().Once();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			Assert.That(executeCount, Is.EqualTo(2));
		}

		[Test]
		public void one_shot_command_should_not_cause_infinite_loop_when_dispatching_to_self()
		{
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate()
			{
				dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			});
			subject.Map(SupportEvent.Type.TYPE1, null).ToCommand<CallbackCommand>().Once();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			// note: no assertion. we just want to know if an error is thrown
		}

		[Test]
		public void commands_should_not_stomp_over_event_mappings()
		{
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate()
			{
				dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE2));
			});
			subject.Map(SupportEvent.Type.TYPE1).ToCommand<CallbackCommand>();
			subject.Map(SupportEvent.Type.TYPE2, null).ToCommand<CallbackCommand>().Once();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			// note: no assertion. we just want to know if an error is thrown
		}

		[Test]
		public void commands_are_executed_in_order()
		{
			subject.Map(SupportEvent.Type.TYPE1).ToCommand<ClassReportingCallbackCommand>();
			subject.Map(SupportEvent.Type.TYPE1).ToCommand<ClassReportingCallbackCommand2>();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			Assert.That(reportedExecutions, Is.EqualTo(new List<object>(){typeof(ClassReportingCallbackCommand), typeof(ClassReportingCallbackCommand2)}).AsCollection);
		}

		[Test]
		public void hooks_are_called()
		{
			Assert.That(hookCallCount(typeof(ClassReportingCallbackHook), typeof(ClassReportingCallbackHook)), Is.EqualTo(2));
		}

		[Test]
		public void command_executes_when_the_guard_allows()
		{
			Assert.That(commandExecutionCountWithGuards(typeof(HappyGuard)), Is.EqualTo(1));
		}

		[Test]
		public void command_executes_when_all_guards_allow()
		{
			Assert.That(commandExecutionCountWithGuards(typeof(HappyGuard), typeof(HappyGuard)), Is.EqualTo(1));
		}

		[Test]
		public void command_does_not_execute_when_the_guard_denies()
		{
			Assert.That(commandExecutionCountWithGuards(typeof(GrumpyGuard)), Is.EqualTo(0));
		}

		[Test]
		public void command_does_not_execute_when_any_guards_denies()
		{
			Assert.That(commandExecutionCountWithGuards(typeof(HappyGuard), typeof(GrumpyGuard)), Is.EqualTo(0));
		}

		[Test]
		public void command_does_not_execute_when_all_guards_deny()
		{
			Assert.That(commandExecutionCountWithGuards(typeof(GrumpyGuard), typeof(GrumpyGuard)), Is.EqualTo(0));
		}

		[Test] //TODO: Make this test pass
		public void event_is_injected_into_guard()
		{
			IEvent injectedEvent = null;
			injector.Map(typeof(Action<EventInjectedCallbackGuard>), "ApproveCallback").ToValue((Action<EventInjectedCallbackGuard>)delegate(EventInjectedCallbackGuard guard)
			{
				injectedEvent = guard.evt;
			});
			subject
				.Map(SupportEvent.Type.TYPE1, typeof(IEvent))
				.ToCommand<NullCommand>()
				.WithGuards<EventInjectedCallbackGuard>();
			SupportEvent  evt = new SupportEvent(SupportEvent.Type.TYPE1);
			dispatcher.Dispatch(evt);
			Assert.That(injectedEvent, Is.EqualTo(evt));
		}

		[Test]
		public void cascading_events_do_not_throw_unmap_errors()
		{
			injector.Map<IEventDispatcher>().ToValue(dispatcher);
			injector.Map<IEventCommandMap>().ToValue(subject);
			subject
				.Map(CascadingCommand.EventType.CASCADING_EVENT)
				.ToCommand<CascadingCommand>().Once();
			dispatcher.Dispatch(new Event(CascadingCommand.EventType.CASCADING_EVENT));
		}

		[Test]
		public void execution_sequence_is_guard_command_guard_command_for_multiple_mappings_to_same_event()
		{
			subject.Map(SupportEvent.Type.TYPE1).ToCommand<ClassReportingCallbackCommand>().WithGuards<ClassReportingCallbackGuard>();
			subject.Map(SupportEvent.Type.TYPE1).ToCommand<ClassReportingCallbackCommand2>().WithGuards<ClassReportingCallbackGuard2>();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			List<object> expectedOrder = new List<object>{typeof(ClassReportingCallbackGuard), typeof(ClassReportingCallbackCommand), typeof(ClassReportingCallbackGuard2), typeof(ClassReportingCallbackCommand2)};
			Assert.That(reportedExecutions, Is.EqualTo(expectedOrder).AsCollection);
		}

		[Test]
		public void previously_constructed_command_does_not_slip_through_the_loop()
		{
			subject.Map(SupportEvent.Type.TYPE1).ToCommand<ClassReportingCallbackCommand>().WithGuards<HappyGuard>();
			subject.Map(SupportEvent.Type.TYPE1).ToCommand<ClassReportingCallbackCommand2>().WithGuards<GrumpyGuard>();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			List<object> expectedOrder = new List<object>{typeof(ClassReportingCallbackCommand)};
			Assert.That(reportedExecutions, Is.EqualTo(expectedOrder).AsCollection);
		}

		[Test] //TODO: Make this unit test work
		public void commands_mapped_during_execution_are_not_executed()
		{
			injector.Map(typeof(IEventCommandMap)).ToValue(subject);	
			injector.Map(typeof(Type), "nestedCommand").ToValue(typeof(ClassReportingCallbackCommand));
			subject.Map(SupportEvent.Type.TYPE1, typeof(IEvent))
				.ToCommand<CommandMappingCommand>().Once();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			Assert.That(reportedExecutions, Is.Empty);
		}

		[Test] //TODO: Make this unit test work
		public void commands_unmapped_during_execution_are_still_executed()
		{
			injector.Map<IEventCommandMap>().ToValue(subject);
			injector.Map(typeof(Type), "nestedCommand").ToValue(typeof(ClassReportingCallbackCommand));
			subject.Map(SupportEvent.Type.TYPE1, typeof(IEvent))
				.ToCommand<CommandUnmappingCommand>().Once();
			subject.Map(SupportEvent.Type.TYPE1, typeof(IEvent))
				.ToCommand<ClassReportingCallbackCommand>().Once();
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			Assert.That(reportedExecutions, Is.EqualTo(new List<object>{typeof(ClassReportingCallbackCommand)}).AsCollection);
		}

		[Test]
		public void mapping_processor_is_called()
		{
			int callCount = 0;
			subject.AddMappingProcessor((CommandMappingList.Processor)delegate(ICommandMapping mapping) {
				callCount++;
			});
			subject.Map(TestEnum.TEST).ToCommand<NullCommand>();
			Assert.That(callCount, Is.EqualTo(1));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private uint commandExecutionCount(int totalEvents = 1, bool oneshot = false)
		{
			uint executeCount = 0;
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate
			{
				executeCount++;
			});
			subject.Map(SupportEvent.Type.TYPE1, typeof(SupportEvent)).ToCommand<CallbackCommand>().Once(oneshot);
			while (totalEvents-- > 0)
			{
				dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			}
			return executeCount;
		}

		private uint oneshotCommandExecutionCount(int totalEvents = 1)
		{
			return commandExecutionCount(totalEvents, true);
		}

		private uint hookCallCount(params object[] hooks)
		{
			uint hookCallCount = 0;

			injector.Unmap(typeof(Action<object>), "ReportingFunction");
			injector.Map(typeof(Action<object>), "ReportingFunction").ToValue((Action<object>)delegate(object hook)
			{
				hookCallCount++;
			});
			subject
				.Map(SupportEvent.Type.TYPE1)
				.ToCommand<NullCommand>()
				.WithHooks(hooks);
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			return hookCallCount;
		}

		private uint commandExecutionCountWithGuards(params object[] guards)
		{
			uint executionCount = 0;
			injector.Map(typeof(Action), "ExecuteCallback").ToValue((Action)delegate()
			{
				executionCount++;
			});
			subject
				.Map(SupportEvent.Type.TYPE1)
				.ToCommand<CallbackCommand>()
				.WithGuards(guards);
			dispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			return executionCount;
		}

		private void reportingFunction(object item)
		{
			reportedExecutions.Add(item);
		}
	}
}


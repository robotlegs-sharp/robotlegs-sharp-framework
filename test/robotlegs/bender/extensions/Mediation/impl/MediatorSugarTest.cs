//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Moq;
using Robotlegs.Bender.Extensions.LocalEventMap.API;
using Robotlegs.Bender.Extensions.Mediation.Impl.Support;
using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using Robotlegs.Bender.Extensions.ViewManagement.Support;
using System;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.EventCommand.Support;
using Robotlegs.Bender.Extensions.LocalEventMap.Impl.Support;
using Robotlegs.Bender.Framework.Impl.LoggingSupport;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.Mediation.Impl
{

	public class MediatorSugarTest
	{
		private Mock<IEventMap> eventMap;

		private Mock<ILogging> logger;

		private SugaryMediator instance;

		private IEventDispatcher eventDispatcher = new EventDispatcher();

		private Enum EVENT_TYPE = CustomEvent.Type.STARTED;

		private Action CALLBACK = delegate (){};

		private SupportEventView view;

		[SetUp]
		public void SetUp()
		{
			instance = new SugaryMediator();
			eventMap = new Mock<IEventMap> ();
			logger = new Mock<ILogging> ();
			instance.eventMap = eventMap.Object;
			instance.logger = logger.Object;
			instance.eventDispatcher = eventDispatcher;
			view = new SupportEventView();
			instance.viewComponent = view;
		}

		[TearDown]
		public void Teardown()
		{
			instance = null;
		}

		[Test]
		public void AddViewListener_With_Basic_View_Component_Throws_Error()
		{
			SupportView basicView = new SupportView ();
			instance.viewComponent = basicView;
			instance.Try_addViewListener (EVENT_TYPE, CALLBACK);
			logger.Verify(_logger=>_logger.Warn(It.IsAny<String>(), It.Is<object>(mediator=>mediator==instance), It.Is<object>(vc=>vc == basicView)), Times.Once);
		}

		[Test]
		public void AddViewListener_Handles_With_Implemented_IEventDispatcher()
		{
			SupportEventViewImplemented implementedView = new SupportEventViewImplemented();
			instance.viewComponent = implementedView;
			instance.Try_addViewListener (EVENT_TYPE, CALLBACK);
			logger.Verify(_logger=>_logger.Warn(It.IsAny<String>(), It.IsAny<object>(), It.IsAny<object>()), Times.Never);
		}

		[Test]
		public void AddViewListener_Handles_With_Property_Of_IEventDispatcher_Called_Dispatcher()
		{
			instance.Try_addViewListener (EVENT_TYPE, CALLBACK);
			logger.Verify(_logger=>_logger.Warn(It.IsAny<String>(), It.IsAny<object>(), It.IsAny<object>()), Times.Never);
		}

		[Test]
		public void AddViewListener_Passes_Vars_To_The_EventMap()
		{
			instance.Try_addViewListener(EVENT_TYPE, CALLBACK);

			eventMap.Verify(_eventMap =>_eventMap.MapListener(
				It.Is<IEventDispatcher>(arg1 => arg1 == view.dispatcher), 
				It.Is<Enum>(arg2 => arg2 == EVENT_TYPE), 
				It.Is<Action>(arg3 => arg3 == CALLBACK),
				It.Is<Type>(arg4 => arg4 == null)), Times.Once);
		}

		[Test]
		public void AddContextListener_Passes_Vars_To_The_EventMap()
		{
			instance.Try_addContextListener(EVENT_TYPE, CALLBACK);

			eventMap.Verify(_eventMap =>_eventMap.MapListener(
				It.Is<IEventDispatcher>(arg1 => arg1 == eventDispatcher), 
				It.Is<Enum>(arg2 => arg2 == EVENT_TYPE), 
				It.Is<Action>(arg3 => arg3 == CALLBACK),
				It.Is<Type>(arg4 => arg4 == null)), Times.Once);
		}

		[Test]
		public void RemoveViewListener_Passes_Vars_To_The_EventMap()
		{
			instance.Try_removeViewListener(EVENT_TYPE, CALLBACK);

			eventMap.Verify(_eventMap =>_eventMap.UnmapListener(
				It.Is<IEventDispatcher>(arg1 => arg1 == view.dispatcher), 
				It.Is<Enum>(arg2 => arg2 == EVENT_TYPE), 
				It.Is<Action>(arg3 => arg3 == CALLBACK),
				It.Is<Type>(arg4 => arg4 == null)), Times.Once);
		}

		[Test]
		public void RemoveContextListener_Passes_Vars_To_The_EventMap()
		{
			instance.Try_removeContextListener(EVENT_TYPE, CALLBACK);

			eventMap.Verify(_eventMap =>_eventMap.UnmapListener(
				It.Is<IEventDispatcher>(arg1 => arg1 == eventDispatcher), 
				It.Is<Enum>(arg2 => arg2 == EVENT_TYPE), 
				It.Is<Action>(arg3 => arg3 == CALLBACK),
				It.Is<Type>(arg4 => arg4 == null)), Times.Once);
		}

		[Test]
		public void Dispatch_DispatchesEvent_On_The_EventDisaptcher()
		{
			bool called = false;
			eventDispatcher.AddEventListener (SupportEvent.Type.TYPE1, (Action)delegate {
				called = true;
			});
			instance.Try_dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			Assert.That (called, Is.True);
		}
	}
}
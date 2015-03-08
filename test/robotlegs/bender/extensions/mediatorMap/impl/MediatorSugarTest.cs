using Moq;
using robotlegs.bender.extensions.localEventMap.api;
using robotlegs.bender.extensions.mediatorMap.impl.support;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.eventDispatcher.impl;
using robotlegs.bender.extensions.viewManager.support;
using System;
using NUnit.Framework;
using robotlegs.bender.extensions.eventCommandMap.support;
using robotlegs.bender.extensions.localEventMap.impl.support;

namespace robotlegs.bender.extensions.mediatorMap.impl
{

	public class MediatorSugarTest
	{
		private Mock<IEventMap> eventMap;

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
			instance.eventMap = eventMap.Object;
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
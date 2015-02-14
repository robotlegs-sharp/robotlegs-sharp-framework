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
		public Mock<IEventMap> eventMap;

		private SugaryMediator instance;

		private MediatorWatcher mediatorWatcher;

		private IEventDispatcher eventDispatcher = new EventDispatcher();

		private SupportView VIEW = new SupportView();
		private Enum EVENT_TYPE = CustomEvent.Type.STARTED;
		private Action CALLBACK = delegate (){};

		private SupportView view;

		[SetUp]
		public void SetUp()
		{
			instance = new SugaryMediator();
			instance.eventMap = eventMap.Object;
			instance.eventDispatcher = eventDispatcher;
			mediatorWatcher = new MediatorWatcher();
			view = new SupportView();
			instance.viewComponent = view;
		}

		[TearDown]
		public void Teardown()
		{
			instance = null;
			mediatorWatcher = null;
		}

		[Test]
		public void AddViewListener_Passes_Vars_To_The_EventMap()
		{
			instance.Try_addViewListener(EVENT_TYPE, CALLBACK);

			Assert.That (true, Is.False);
			/*
			assertThat(eventMap, received().method('mapListener')
											.args(	strictlyEqualTo(view),
													strictlyEqualTo(EVENT_STRING),
													strictlyEqualTo(CALLBACK),
													strictlyEqualTo(EVENT_CLASS)));
			//*/
		}

		[Test]
		public void AddContextListener_Passes_Vars_To_The_EventMap()
		{
			instance.Try_addContextListener(EVENT_TYPE, CALLBACK);

			Assert.That (true, Is.False);
			/*
			assertThat(eventMap, received().method('mapListener')
											.args(	strictlyEqualTo(eventDispatcher),
													strictlyEqualTo(EVENT_STRING),
													strictlyEqualTo(CALLBACK),
													strictlyEqualTo(EVENT_CLASS)));
			//*/
		}

		[Test]
		public void RemoveViewListener_Passes_Vars_To_The_EventMap()
		{
			instance.Try_removeViewListener(EVENT_TYPE, CALLBACK);

			Assert.That (true, Is.False);
			/*
			assertThat(eventMap, received().method('unmapListener')
											.args(	strictlyEqualTo(view),
													strictlyEqualTo(EVENT_STRING),
													strictlyEqualTo(CALLBACK),
													strictlyEqualTo(EVENT_CLASS)));
		//*/
		}

		[Test]
		public void RemoveContextListener_Passes_Vars_To_The_EventMap()
		{
			instance.Try_removeContextListener(EVENT_TYPE, CALLBACK);

			Assert.That (true, Is.False);
			/*
			assertThat(eventMap, received().method('unmapListener')
											.args(	strictlyEqualTo(eventDispatcher),
													strictlyEqualTo(EVENT_STRING),
													strictlyEqualTo(CALLBACK),
													strictlyEqualTo(EVENT_CLASS)));
			//*/
		}

		[Test]
		public void Dispatch_DispatchesEvent_On_The_EventDisaptcher()
		{
			//Async.handleEvent(this, eventDispatcher, Event.COMPLETE, benignHandler);
			instance.Try_dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			Assert.That (true, Is.False);
		}
		/*
		protected function benignHandler(e:Event, o:Object):void
		{

		}
		//*/
	}
}
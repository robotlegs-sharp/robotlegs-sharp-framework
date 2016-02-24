//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//  Copyright (c) 2011 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.LocalEventMap.API;
using Robotlegs.Bender.Extensions.LocalEventMap.Impl.Support;
using System;

namespace Robotlegs.Bender.Extensions.LocalEventMap.Impl
{
	public class EventMapTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private enum EventType
		{
			COMPLETE,
			CHANGE
		}

		private IEventDispatcher eventDispatcher;

		private IEventMap eventMap;

		private bool listenerExecuted;

		private uint listenerExecutedCount;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void runBeforeEachTest()
		{
			eventDispatcher = new EventDispatcher();
			eventMap = new EventMap();
		}

		[TearDown]
		public void runAfterEachTest()
		{
			resetListenerExecuted();
			resetListenerExecutedCount();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void listener_mapped_without_type_is_triggered_by_plain_Event()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventDispatcher.Dispatch(new Event(CustomEvent.Type.STARTED));
			Assert.That(listenerExecuted, Is.True);
		}

		[Test]
		public void listener_mapped_without_type_is_triggered_by_correct_typed_event()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			Assert.That(listenerExecuted, Is.True);
		}

		[Test]
		public void listener_mapped_with_type_is_triggered_by_correct_typed_event()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener, typeof(CustomEvent));
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			Assert.That(listenerExecuted, Is.True);
		}

		[Test]
		public void listener_mapped_with_type_is_NOT_triggered_by_plain_event()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener, typeof(CustomEvent));
			eventDispatcher.Dispatch(new Event(CustomEvent.Type.STARTED));
			Assert.That(listenerExecuted, Is.False);
		}

		[Test]
		public void listener_mapped_twice_only_fires_once()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listenerWithCounter, typeof(CustomEvent));
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listenerWithCounter, typeof(CustomEvent));
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			Assert.That(listenerExecutedCount, Is.EqualTo(1));
		}

		[Test]
		public void listener_mapped_twice_and_removed_once_doesnt_fire()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listenerWithCounter, typeof(CustomEvent));
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listenerWithCounter, typeof(CustomEvent));
			eventMap.UnmapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listenerWithCounter, typeof(CustomEvent));
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			Assert.That(listenerExecutedCount, Is.EqualTo(0));
		}

		[Test]
		public void listener_mapped_and_unmapped_without_type_doesnt_fire_in_response_to_typed_or_plain_event()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventMap.UnmapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventDispatcher.Dispatch(new Event(CustomEvent.Type.STARTED));
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			Assert.That(listenerExecuted, Is.False);
		}

		[Test]
		public void listener_mapped_and_unmapped_with_type_doesnt_fire_in_response_to_typed_or_plain_event()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener, typeof(CustomEvent));
			eventMap.UnmapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener, typeof(CustomEvent));
			eventDispatcher.Dispatch(new Event(CustomEvent.Type.STARTED));
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			Assert.That(listenerExecuted, Is.False);
		}

		[Test]
		public void listener_mapped_with_type_and_unmapped_without_type_fires_in_response_to_typed_event()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener, typeof(CustomEvent));
			eventMap.UnmapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			Assert.That(listenerExecuted, Is.True);
		}

		[Test]
		public void listener_mapped_without_type_and_unmapped_with_type_fires_in_response_to_plain_event()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventMap.UnmapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener, typeof(CustomEvent));
			eventDispatcher.Dispatch(new Event(CustomEvent.Type.STARTED));
			Assert.That(listenerExecuted, Is.True);
		}

		[Test]
		public void unmapListeners_causes_no_handlers_to_fire()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventMap.MapListener(eventDispatcher, EventType.COMPLETE, (Action<IEvent>)listener);
			eventMap.MapListener(eventDispatcher, EventType.CHANGE, (Action<IEvent>)listener);
			eventMap.UnmapListeners();
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			eventDispatcher.Dispatch(new Event(EventType.COMPLETE));
			eventDispatcher.Dispatch(new Event(EventType.CHANGE));
			Assert.That(listenerExecuted, Is.False);
		}

		[Test]
		public void suspend_causes_no_handlers_to_fire()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventMap.MapListener(eventDispatcher, EventType.COMPLETE, (Action<IEvent>)listener);
			eventMap.MapListener(eventDispatcher, EventType.CHANGE, (Action<IEvent>)listener);
			eventMap.Suspend();
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			eventDispatcher.Dispatch(new Event(EventType.COMPLETE));
			eventDispatcher.Dispatch(new Event(EventType.CHANGE));
			Assert.That(listenerExecuted, Is.False);
		}

		[Test]
		public void suspend_then_resume_restores_handlers_to_fire()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listenerWithCounter);
			eventMap.MapListener(eventDispatcher, EventType.COMPLETE, (Action<IEvent>)listenerWithCounter);
			eventMap.MapListener(eventDispatcher, EventType.CHANGE, (Action<IEvent>)listenerWithCounter);
			eventMap.Suspend();
			eventMap.Resume();
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			eventDispatcher.Dispatch(new Event(EventType.COMPLETE));
			eventDispatcher.Dispatch(new Event(EventType.CHANGE));
			Assert.That (listenerExecutedCount, Is.EqualTo (3));
		}

		[Test]
		public void listeners_added_while_suspended_dont_fire()
		{
			eventMap.Suspend();
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventMap.MapListener(eventDispatcher, EventType.COMPLETE, (Action<IEvent>)listener);
			eventMap.MapListener(eventDispatcher, EventType.CHANGE, (Action<IEvent>)listener);
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			eventDispatcher.Dispatch(new Event(EventType.COMPLETE));
			eventDispatcher.Dispatch(new Event(EventType.CHANGE));
			Assert.That(listenerExecuted, Is.False);
		}

		[Test]
		public void listeners_added_while_suspended_fire_after_resume()
		{
			eventMap.Suspend();
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listenerWithCounter);
			eventMap.MapListener(eventDispatcher, EventType.COMPLETE, (Action<IEvent>)listenerWithCounter);
			eventMap.MapListener(eventDispatcher, EventType.CHANGE, (Action<IEvent>)listenerWithCounter);
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			eventMap.Resume();
			eventDispatcher.Dispatch(new Event(EventType.COMPLETE));
			eventDispatcher.Dispatch(new Event(EventType.CHANGE));
			Assert.That(listenerExecutedCount, Is.EqualTo(2));
		}

		[Test]
		public void listeners_can_be_unmapped_while_suspended()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listenerWithCounter);
			eventMap.MapListener(eventDispatcher, EventType.COMPLETE, (Action<IEvent>)listenerWithCounter);
			eventMap.MapListener(eventDispatcher, EventType.CHANGE, (Action<IEvent>)listenerWithCounter);
			eventMap.Suspend();
			eventMap.UnmapListener(eventDispatcher, EventType.CHANGE, (Action<IEvent>)listenerWithCounter);
			eventMap.Resume();
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			eventDispatcher.Dispatch(new Event(EventType.COMPLETE));
			eventDispatcher.Dispatch(new Event(EventType.CHANGE));
			Assert.That(listenerExecutedCount, Is.EqualTo(2));
		}

		[Test]
		public void all_listeners_can_be_unmapped_while_suspended()
		{
			eventMap.MapListener(eventDispatcher, CustomEvent.Type.STARTED, (Action<IEvent>)listener);
			eventMap.MapListener(eventDispatcher, EventType.COMPLETE, (Action<IEvent>)listener);
			eventMap.MapListener(eventDispatcher, EventType.CHANGE, (Action<IEvent>)listener);
			eventMap.Suspend();
			eventMap.UnmapListeners();
			eventMap.Resume();
			eventDispatcher.Dispatch(new CustomEvent(CustomEvent.Type.STARTED));
			eventDispatcher.Dispatch(new Event(EventType.COMPLETE));
			eventDispatcher.Dispatch(new Event(EventType.CHANGE));
			Assert.That(listenerExecuted, Is.False);
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected void listener(IEvent e)
		{
			listenerExecuted = true;
		}

		protected void resetListenerExecuted()
		{
			listenerExecuted = false;
		}

		protected void listenerWithCounter(IEvent e)
		{
			listenerExecutedCount++;
		}

		protected void resetListenerExecutedCount()
		{
			listenerExecutedCount = 0;
		}
	}
}

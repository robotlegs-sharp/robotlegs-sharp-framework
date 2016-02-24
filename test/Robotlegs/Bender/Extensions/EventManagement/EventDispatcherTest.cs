//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.EventManagement.API;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.EventManagement.Support;

namespace Robotlegs.Bender.Extensions.EventManagement.Impl
{
	[TestFixture]
	public class EventDispatcherTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private enum Type
		{
			A,
			B,
			C
		}

		private IEventDispatcher dispatcher;

		private List<object> reported;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			dispatcher = new EventDispatcher ();
			reported = new List<object>();
		}


		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void listener_gets_called_as_action()
		{
			int callCount = 0;
			dispatcher.AddEventListener(Type.A, (Action)delegate {
				callCount++;
			});
			dispatcher.Dispatch (new BlankEvent (Type.A));
			Assert.That (callCount, Is.EqualTo (1));
		}

		[Test]
		public void listener_gets_called_multiple_times()
		{
			int callCount = 0;
			dispatcher.AddEventListener(Type.A, (Action)delegate {
				callCount++;
			});
			dispatcher.Dispatch (new BlankEvent (Type.A));
			dispatcher.Dispatch (new BlankEvent (Type.A));
			dispatcher.Dispatch (new BlankEvent (Type.A));
			Assert.That (callCount, Is.EqualTo (3));
		}

		[Test]
		public void listener_not_called_if_incorrect_key()
		{
			int callCount = 0;
			dispatcher.AddEventListener(Type.A, (Action)delegate {
				callCount++;
			});
			dispatcher.Dispatch (new BlankEvent (Type.B));
			Assert.That (callCount, Is.EqualTo (0));
		}

		[Test]
		public void listener_not_called_if_removed()
		{
			int callCount = 0;
			Action action = (Action)delegate
			{
				callCount++;
			};
			dispatcher.AddEventListener (Type.A, action);
			dispatcher.RemoveEventListener(Type.A, action);
			dispatcher.Dispatch (new BlankEvent (Type.A));
			Assert.That (callCount, Is.EqualTo (0));
		}

		[Test]
		public void remove_all_removes_all_listeners()
		{
			int callCount = 0;
			Action action = (Action)delegate
			{
				callCount++;
			};
			dispatcher.AddEventListener (Type.A, action);
			dispatcher.AddEventListener (Type.B, action);
			dispatcher.AddEventListener (Type.C, action);
			dispatcher.RemoveAllEventListeners ();
			dispatcher.Dispatch (new BlankEvent (Type.A));
			dispatcher.Dispatch (new BlankEvent (Type.B));
			dispatcher.Dispatch (new BlankEvent (Type.C));
			Assert.That (callCount, Is.EqualTo (0));
		}

		[Test]
		public void add_listener_returns_true_for_has_listener()
		{
			Assert.That (dispatcher.HasEventListener (Type.A), Is.False);
			dispatcher.AddEventListener (Type.A, (Action)delegate{});
			Assert.That (dispatcher.HasEventListener (Type.A), Is.True);
		}

		[Test]
		public void add_listener_returns_false_after_removed_listener()
		{
			Action action = (Action)delegate{};
			dispatcher.AddEventListener (Type.A, action);
			dispatcher.RemoveEventListener (Type.A, action);
			Assert.That (dispatcher.HasEventListener (Type.A), Is.False);
		}

		[Test]
		public void remove_listener_removes_correct_types()
		{
			object a = new object ();
			object b = new object ();
			object c = new object ();
			Action reportB = Report (b);
			dispatcher.AddEventListener (Type.A, Report (a));
			dispatcher.AddEventListener (Type.B, reportB);
			dispatcher.AddEventListener (Type.C, Report (c));
			dispatcher.RemoveEventListener (Type.B, reportB);
			dispatcher.Dispatch (new BlankEvent (Type.C));
			dispatcher.Dispatch (new BlankEvent (Type.B));
			dispatcher.Dispatch (new BlankEvent (Type.A));
			Assert.That (reported, Is.EqualTo (new List<object> (){ c, a }).AsCollection);
		}

		[Test]
		public void check_custom_event_data_gets_passed()
		{
			string message = null;
			dispatcher.AddEventListener(CustomEvent.Type.B, (Action<CustomEvent>)delegate (CustomEvent evt)
			{
				message = evt.message;
			});
			dispatcher.Dispatch(new CustomEvent(CustomEvent.Type.B, "hello world"));
			Assert.That (message, Is.EqualTo ("hello world"));
		}

		[Test]
		public void check_custom_event_can_be_downcast()
		{
			IEvent actual = null;
			dispatcher.AddEventListener(CustomEvent.Type.B, (Action<IEvent>)delegate (IEvent evt)
			{
				actual = evt;
			});
			dispatcher.Dispatch(new CustomEvent(CustomEvent.Type.B, "hello ievent"));
			Assert.That (actual, Is.Not.Null);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private Action Report(object message)
		{
			return delegate
			{
				reported.Add(message);
			};
		}
	}
}


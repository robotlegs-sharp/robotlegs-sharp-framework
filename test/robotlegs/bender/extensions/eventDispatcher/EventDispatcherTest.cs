using System;
using NUnit.Framework;
using robotlegs.bender.extensions.eventDispatcher.api;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.eventDispatcher.impl
{
	[TestFixture]
	public class EventDispatcherTest
	{
		private enum Type
		{
			A,
			B,
			C
		}

		private IEventDispatcher ed;
		private List<object> list = null;
		private int count = 0;

		[SetUp]
		public void Setup()
		{
			ed = new EventDispatcher ();
			list = new List<object>();
		}

		[Test]
		public void QuickTest()
		{
			ed.AddEventListener(Type.A, Listener);
			ed.AddEventListener(Type.A, Listener);
			ed.AddEventListener(Type.A, Listener);
			Assert.That (list, Is.EqualTo (new List<object> ()).AsCollection);
			ed.Dispatch (new BlankEvent (Type.A));
			Assert.That (list, Is.EqualTo (new List<object> {0,1,2}).AsCollection);
		}

		private void Listener(IEvent evt)
		{
			list.Add (count++);
		}
	}

	public class BlankEvent : Event
	{
		public BlankEvent(Enum type):base(type)
		{

		}
	}
}


using System;
using NUnit.Framework;
using robotlegs.bender.framework.impl.loggingSupport;
using System.Collections.Generic;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	[TestFixture]
	public class PinTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Pin pin;

		private object instance;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			instance = new object();
			pin = new Pin();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		//*
		[Test]
		public void detain_dispatches_event()
		{
			int eventCount = 0;
			pin.Detained += delegate(object obj) {
				eventCount++;
			};
			pin.Detain(instance);
			Assert.AreEqual(eventCount, 1);
		}

		[Test]
		public void detain_dispatches_event_once_per_valid_detainment()
		{
			int eventCount = 0;
			pin.Detained += delegate(object obj) {
				eventCount++;
			};
			pin.Detain(instance);
			pin.Detain(instance);
			Assert.AreEqual(eventCount, 1);
		}

		[Test]
		public void release_dispatches_event()
		{
			int eventCount = 0;
			pin.Released += delegate(object obj) {
				eventCount++;
			};
			pin.Detain(instance);
			pin.Release(instance);
			Assert.AreEqual(eventCount, 1);
		}

		[Test]
		public void release_dispatches_event_once_per_valid_release()
		{

			int eventCount = 0;
			pin.Released += delegate(object obj) {
				eventCount++;
			};
			pin.Detain(instance);
			pin.Release(instance);
			pin.Release(instance);
			Assert.AreEqual(eventCount, 1);
		}

		[Test]
		public void release_does_not_dispatch_event_if_instance_was_not_detained()
		{
			int eventCount = 0;
			pin.Released += delegate(object obj) {
				eventCount++;
			};
			pin.Release(instance);
			Assert.AreEqual(eventCount, 0);
		}

		[Test]
		public void releaseAll_dispatches_events_for_all_instances()
		{
			List<object> releasedObjects = new List<object> ();
			pin.Released += delegate(object obj) {
				releasedObjects.Add(obj);
			};
			object instanceA = new object();
			object instanceB = new object();
			object instanceC = new object();
			pin.Detain(instanceA);
			pin.Detain(instanceB);
			pin.Detain(instanceC);
			pin.ReleaseAll();
			object[] instanceABC = new object[]{ instanceA, instanceB, instanceC };

			Assert.That (releasedObjects.ToArray(), Is.EqualTo (instanceABC).AsCollection);
		}
	}
}
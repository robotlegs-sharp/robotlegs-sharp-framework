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
		/* Public Properties                                                          */
		/*============================================================================*/

//		[Rule]
//		public var mocks:MockolateRule = new MockolateRule();

//		[Mock]
//		public var dispatcher:IEventDispatcher;

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
			instance = new object()
			pin = new Pin(dispatcher);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		//*
		[Test]
		public void detain_dispatches_event()
		{
//			pin.detain(instance);
//			assertThat(dispatcher, received()
//				.method('dispatchEvent')
//				.arg(pinEventMatcher(PinEvent.DETAIN))
//				.once());
		}

		/*
		[Test]
		public function detain_dispatches_event_once_per_valid_detainment():void
		{
			pin.detain(instance);
			pin.detain(instance);
			assertThat(dispatcher, received()
				.method('dispatchEvent')
				.arg(pinEventMatcher(PinEvent.DETAIN))
				.once());
		}

		[Test]
		public function release_dispatches_event():void
		{
			pin.detain(instance);
			pin.release(instance);
			assertThat(dispatcher, received()
				.method('dispatchEvent')
				.arg(pinEventMatcher(PinEvent.RELEASE))
				.once());
		}

		[Test]
		public function release_dispatches_event_once_per_valid_release():void
		{
			pin.detain(instance);
			pin.release(instance);
			pin.release(instance);
			assertThat(dispatcher, received()
				.method('dispatchEvent')
				.arg(pinEventMatcher(PinEvent.RELEASE))
				.once());
		}

		[Test]
		public function release_does_not_dispatch_event_if_instance_was_not_detained():void
		{
			pin.release(instance);
			assertThat(dispatcher, received()
				.method('dispatchEvent')
				.arg(pinEventMatcher(PinEvent.RELEASE))
				.never());
		}

		[Test]
		public function releaseAll_dispatches_events_for_all_instances():void
		{
			const instanceA:Object = {};
			const instanceB:Object = {};
			const instanceC:Object = {};
			pin.detain(instanceA);
			pin.detain(instanceB);
			pin.detain(instanceC);
			pin.releaseAll();
			assertThat(dispatcher, received()
				.method('dispatchEvent')
				.arg(allOf(instanceOf(PinEvent), hasProperties({
					type: PinEvent.RELEASE,
					instance: anyOf(
						equalTo(instanceA),
						equalTo(instanceB),
						equalTo(instanceC))})))
				.thrice());
		}
		*/

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		/*
		private function pinEventMatcher(type:String):Matcher
		{
			return allOf(
				instanceOf(PinEvent),
				hasProperties({type: type, instance: instance}));
		}
		*/
	}
}
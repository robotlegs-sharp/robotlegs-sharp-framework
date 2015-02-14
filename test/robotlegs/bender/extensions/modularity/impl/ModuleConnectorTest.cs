using robotlegs.bender.extensions.eventDispatcher;
using robotlegs.bender.extensions.modularity.api;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using NUnit.Framework;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.eventCommandMap.support;
using System;

namespace robotlegs.bender.extensions.modularity.impl
{

	public class ModuleConnectorTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IEventDispatcher parentDispatcher;

		private IEventDispatcher childADispatcher;

		private IEventDispatcher childBDispatcher;

		private IModuleConnector parentConnector;

		private IModuleConnector childAConnector;

		private IModuleConnector childBConnector;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			IContext parentContext = new Context().Install(typeof(EventDispatcherExtension));
			IContext childAContext = new Context().Install(typeof(EventDispatcherExtension));
			IContext childBContext = new Context().Install(typeof(EventDispatcherExtension));

			// TODO: Matt, Create

//			parentContext.addChild(childAContext);
//			parentContext.addChild(childBContext);

			parentConnector = new ModuleConnector(parentContext);
			childAConnector = new ModuleConnector(childAContext);
			childBConnector = new ModuleConnector(childBContext);

			parentDispatcher = parentContext.injector.GetInstance(typeof(IEventDispatcher)) as IEventDispatcher;
			childADispatcher = childAContext.injector.GetInstance(typeof(IEventDispatcher)) as IEventDispatcher;
			childBDispatcher = childBContext.injector.GetInstance(typeof(IEventDispatcher)) as IEventDispatcher;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Allows_Communication_From_Parent_To_Child()
		{
			parentConnector.OnDefaultChannel()
				.RelayEvent(SupportEvent.Type.TYPE1);
			childAConnector.OnDefaultChannel()
				.ReceiveEvent(SupportEvent.Type.TYPE1);

			bool wasCalled = false;
			childADispatcher.AddEventListener(SupportEvent.Type.TYPE1, delegate(IEvent obj) {
				wasCalled = true;
			});

			parentDispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));

			Assert.That(wasCalled, Is.True);
		}

		[Test]
		public void Allows_Communication_From_Child_To_Parent()
		{
			parentConnector.OnDefaultChannel()
				.ReceiveEvent(SupportEvent.Type.TYPE1);
			childAConnector.OnDefaultChannel()
				.RelayEvent(SupportEvent.Type.TYPE1);

			bool wasCalled = false;
			parentDispatcher.AddEventListener(SupportEvent.Type.TYPE1, delegate(IEvent evt) {
				wasCalled = true;
			});

			childADispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));

			Assert.That (wasCalled, Is.True);
		}

		[Test]
		public void Allows_Communication_Amongst_Children()
		{
			childAConnector.OnDefaultChannel()
				.RelayEvent(SupportEvent.Type.TYPE1);
			childBConnector.OnDefaultChannel()
				.ReceiveEvent(SupportEvent.Type.TYPE1);

			bool wasCalled = false;
			childBDispatcher.AddEventListener(SupportEvent.Type.TYPE1, delegate(IEvent evt) {
				wasCalled = true;
			});

			childADispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));

			Assert.That (wasCalled, Is.True);
		}

		[Test]
		public void Channels_Are_Isolated()
		{
			parentConnector.OnDefaultChannel()
				.RelayEvent(SupportEvent.Type.TYPE1);
			childAConnector.OnChannel("other-channel")
				.ReceiveEvent(SupportEvent.Type.TYPE1);

			bool wasCalled =false;
			childADispatcher.AddEventListener(SupportEvent.Type.TYPE1, delegate(IEvent evt) {
				wasCalled = true;
			});

			parentDispatcher.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));

			Assert.That (wasCalled, Is.False);
		}
	}
}

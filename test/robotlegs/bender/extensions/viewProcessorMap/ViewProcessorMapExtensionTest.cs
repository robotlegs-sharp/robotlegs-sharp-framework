using System;
using NUnit.Framework;
using robotlegs.bender.extensions.eventDispatcher.api;
using System.Collections.Generic;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewProcessorMap.api;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.viewProcessorMap
{
	[TestFixture]
	public class ViewProcessorMapExtensionTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Context context;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			context = new Context();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/
		[Test, ExpectedException(typeof(LifecycleException))]
		public void Installing_After_Initialization_Throws_Error()
		{
			context.Initialize();
			context.Install(typeof(ViewProcessorMapExtension));
		}

		[Test]
		public void ViewProcessorMap_Is_Mapped_Into_Injector_On_Initialize()
		{
			Object actual = null;
			context.Install (typeof(ViewManagerExtension));
			context.Install (typeof(ViewProcessorMapExtension));
			context.WhenInitializing( delegate() {
				actual = context.injector.GetInstance(typeof(IViewProcessorMap));
			});
			context.Initialize();
			Assert.That(actual, Is.InstanceOf(typeof(IViewProcessorMap)));
		}


		[Test]
		public void ViewProcessorMap_Is_Unmapped_From_Injector_On_Destroy()
		{
			context.Install (typeof(ViewManagerExtension));
			context.Install (typeof(ViewProcessorMapExtension));
			context.AfterDestroying( delegate {
				Assert.That(context.injector.SatisfiesDirectly(typeof(IViewProcessorMap)), Is.False);
			});
			context.Initialize();
			context.Destroy();
		}
	}
}

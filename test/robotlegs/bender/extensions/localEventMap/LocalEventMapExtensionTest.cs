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
using robotlegs.bender.framework.impl;
using NUnit.Framework;
using robotlegs.bender.extensions.localEventMap.api;
using robotlegs.bender.extensions.eventDispatcher;

namespace robotlegs.bender.extensions.localEventMap
{
	public class LocalEventMapExtensionTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Context context;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			context = new Context();
			context.Install<EventDispatcherExtension> ();
			context.Install<LocalEventMapExtension>();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void localEventMap_is_mapped_into_injector()
		{
			object actual = null;
			context.WhenInitializing(delegate() {
				actual = context.injector.GetInstance(typeof(IEventMap));
			});
			context.Initialize();
			Assert.That(actual, Is.InstanceOf<IEventMap>());
		}
	}
}

//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.ViewManagement.Support;


namespace Robotlegs.Bender.Extensions.ViewProcessor.Impl
{
	public class ViewInjectionProcessorTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector injector;

		private ViewInjectionProcessor viewInjector;

		private object injectionValue;

		private ViewWithInjection view;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			injector = new RobotlegsInjector();
			viewInjector = new ViewInjectionProcessor();
			injectionValue = MapSpriteForInjection();
			view = new ViewWithInjection();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void ProcessFulfillsInjectionsWhenClassPassed()
		{
			viewInjector.Process(view, view.GetType(), injector);
			Assert.That(view.InjectedSprite, Is.EqualTo(injectionValue));
		}

		[Test]
		public void ProcessFulfillsInjectionsWhenClassNotPassed()
		{
			viewInjector.Process(view, null, injector);
			Assert.That(view.InjectedSprite, Is.EqualTo(injectionValue));
		}

		[Test]
		public void ProcessDoesNotRerunInjections()
		{
			viewInjector.Process(view, typeof(ViewWithInjection), injector);

			injector.Unmap(typeof(object));
			injector.Map(typeof(object)).ToValue(new SupportView());

			viewInjector.Process(view, typeof(ViewWithInjection), injector);

			Assert.That(view.InjectedSprite, Is.EqualTo(injectionValue));
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected object MapSpriteForInjection()
		{
			object injectionValue = new object();
			injector.Map(typeof(object)).ToValue(injectionValue);
			return injectionValue;
		}
	}
}

class ViewWithInjection
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public object InjectedSprite { get; set; }
}


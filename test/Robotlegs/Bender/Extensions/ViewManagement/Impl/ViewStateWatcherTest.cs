//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using NUnit.Framework;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Extensions.ViewManagement.Support;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Extensions.ContextViews.Impl;
using swiftsuspenders.errors;

namespace Robotlegs.Bender.Extensions.ViewManagement.Impl
{
	[TestFixture]
	public class ViewStateWatcherTest
	{
		/*============================================================================*/
		/* Private Variables	                                                      */
		/*============================================================================*/

		private IContext context;

		private SupportView contextViewObject;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			context = new Context().Install(typeof(TestSupportViewStateWatcherExtension));
			contextViewObject = new SupportView ();
		}

		[TearDown]
		public void After()
		{
			if(context != null && context.Initialized) context.Destroy ();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void ViewStateWatcherExtension_Is_Mapped_To_Injector_After_Context_View_Is_Configured()
		{
			object actual = null;
			context.Configure (new ContextView (contextViewObject));
			actual = context.injector.GetInstance<IViewStateWatcher>();

			Assert.That(actual, Is.InstanceOf<IViewStateWatcher>());
		}


		[Test, ExpectedException(typeof(InjectorMissingMappingException))]
		public void ViewStateWatcherExtension_Is_Not_Mapped_To_Injector_Without_A_Context_View()
		{
			object actual = context.injector.GetInstance<IViewStateWatcher>();
		}

		[Test]
		public void ViewStateWatcher_Knowns_When_View_Is_Added()
		{
			context.Configure (new ContextView (contextViewObject));
			IViewStateWatcher actual = context.injector.GetInstance<IViewStateWatcher>() as IViewStateWatcher;

			Assert.That (actual.isAdded, Is.False);
			contextViewObject.AddThisView ();
			Assert.That (actual.isAdded, Is.True);
		}

		[Test]
		public void ViewStateWatcher_Hears_When_View_Is_Added()
		{
			bool heardEvent = false;
			context.Configure (new ContextView (contextViewObject));
			IViewStateWatcher actual = context.injector.GetInstance<IViewStateWatcher>() as IViewStateWatcher;
			actual.added += delegate(object obj) {
				heardEvent = true;
			};
			contextViewObject.AddThisView ();

			Assert.That (heardEvent, Is.True);
		}

		[Test]
		public void ViewStateWatcher_Hears_When_View_Is_Removed()
		{
			bool heardEvent = false;
			context.Configure (new ContextView (contextViewObject));
			IViewStateWatcher actual = context.injector.GetInstance<IViewStateWatcher>() as IViewStateWatcher;
			actual.removed += delegate(object obj) {
				heardEvent = true;
			};
			contextViewObject.RemoveThisView ();

			Assert.That (heardEvent, Is.True);
		}

		[Test]
		public void ViewStateWatcher_Hears_When_View_Is_Enabled()
		{
			bool heardEvent = false;
			context.Configure (new ContextView (contextViewObject));
			IViewStateWatcher actual = context.injector.GetInstance<IViewStateWatcher>() as IViewStateWatcher;
			actual.enabled += delegate(object obj) {
				heardEvent = true;
			};
			contextViewObject.Enable ();

			Assert.That (heardEvent, Is.True);
		}

		[Test]
		public void ViewStateWatcher_Hears_When_View_Is_Disabled()
		{
			bool heardEvent = false;
			context.Configure (new ContextView (contextViewObject));
			IViewStateWatcher actual = context.injector.GetInstance<IViewStateWatcher>() as IViewStateWatcher;
			actual.disabled += delegate(object obj) {
				heardEvent = true;
			};
			contextViewObject.Disable ();

			Assert.That (heardEvent, Is.True);
		}

		[Test]
		public void ViewStateWatcher_Passes_View_Through_Added_Event()
		{
			object passedObject = null;
			context.Configure (new ContextView (contextViewObject));
			IViewStateWatcher actual = context.injector.GetInstance<IViewStateWatcher>() as IViewStateWatcher;
			actual.added += delegate(object obj) {
				passedObject = obj;
			};
			contextViewObject.AddThisView ();

			Assert.That (passedObject, Is.EqualTo(contextViewObject));
		}

		[Test]
		public void ViewStateWatcher_Passes_View_Through_Removed_Event()
		{
			object passedObject = null;
			context.Configure (new ContextView (contextViewObject));
			IViewStateWatcher actual = context.injector.GetInstance<IViewStateWatcher>() as IViewStateWatcher;
			actual.removed += delegate(object obj) {
				passedObject = obj;
			};
			contextViewObject.RemoveThisView ();

			Assert.That (passedObject, Is.EqualTo(contextViewObject));
		}

		[Test]
		public void ViewStateWatcher_Passes_View_Through_Enabled_Event()
		{
			object passedObject = null;
			context.Configure (new ContextView (contextViewObject));
			IViewStateWatcher actual = context.injector.GetInstance<IViewStateWatcher>() as IViewStateWatcher;
			actual.enabled += delegate(object obj) {
				passedObject = obj;
			};
			contextViewObject.Enable ();

			Assert.That (passedObject, Is.EqualTo(contextViewObject));
		}

		[Test]
		public void ViewStateWatcher_Passes_View_Through_Disabled_Event()
		{
			object passedObject = null;
			context.Configure (new ContextView (contextViewObject));
			IViewStateWatcher actual = context.injector.GetInstance<IViewStateWatcher>() as IViewStateWatcher;
			actual.disabled += delegate(object obj) {
				passedObject = obj;
			};
			contextViewObject.Disable();

			Assert.That (passedObject, Is.EqualTo(contextViewObject));
		}
	}
}


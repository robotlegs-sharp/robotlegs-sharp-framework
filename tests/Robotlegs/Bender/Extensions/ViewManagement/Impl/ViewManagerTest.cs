//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.ViewManagement.Support;
using Robotlegs.Bender.Extensions.ViewManagement.API;

namespace Robotlegs.Bender.Extensions.ViewManagement.Impl
{
	public class ViewManagerTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private SupportContainer container;

		private ContainerRegistry registry;

		private ViewManager viewManager;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			container = new SupportContainer ();
			registry = new ContainerRegistry();
			registry.SetParentFinder (new SupportParentFinder ());
			viewManager = new ViewManager(registry);
			ViewNotifier.SetRegistry (registry);
		}

		[TearDown]
		public void after()
		{
			registry.SetParentFinder (null);
			ViewNotifier.SetRegistry (null);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void addContainer()
		{
			viewManager.AddContainer(new SupportView());
		}

		[Test]
		public void addContainer_throws_if_containers_are_nested()
		{
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
            {
                SupportContainer container1 = new SupportContainer();
                SupportContainer container2 = new SupportContainer();
                container1.AddChild(container2);
                viewManager.AddContainer(container1);
                viewManager.AddContainer(container2);
            }
            ));
		}

		[Test]
		public void handler_is_called()
		{
			SupportView expected = new SupportView();
			object actual = null;
			viewManager.AddContainer(container);
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				actual = view;
			}));
			container.AddChild(expected);
			expected.Register ();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void handlers_are_called()
		{
			List<object> expected = new List<object> {"handler1", "handler2", "handler3"};
			List<object> actual = new List<object>();
			viewManager.AddContainer(container);
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				actual.Add("handler1");
			}));
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				actual.Add("handler2");
			}));
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				actual.Add("handler3");
			}));
			SupportView supportView = new SupportView();
			container.AddChild(supportView);
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void handler_is_not_called_after_container_removal()
		{
			int callCount = 0;
			viewManager.AddContainer(container);
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				callCount++;
			}));
			viewManager.RemoveContainer(container);
			SupportView supportView = new SupportView();
			container.AddChild(supportView);
			Assert.That(callCount, Is.EqualTo(0));
		}

		[Test]
		public void handler_is_not_called_after_removeAll()
		{
			int callCount = 0;
			viewManager.AddContainer(container);
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				callCount++;
			}));
			viewManager.RemoveAllHandlers();
			container.AddChild(new SupportView());
			Assert.That(callCount, Is.EqualTo(0));
		}

		[Test]
		public void fallback_handles_when_container_is_not_parent()
		{
			int callCount = 0;
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				callCount++;
			}));
			viewManager.SetFallbackContainer(new object());
			SupportContainer newContainer = new SupportContainer ();
			newContainer.AddChild(new SupportView());
			Assert.That (callCount, Is.EqualTo (1));
		}

		[Test]
		public void adding_fallback_doesnt_duplicate_handlers()
		{
			int callCount = 0;
			viewManager.AddContainer(container);
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				callCount++;
			}));
			viewManager.SetFallbackContainer(new object());
			container.AddChild(new SupportView());
			Assert.That (callCount, Is.EqualTo (1));
		}

		[Test]
		public void adding_and_removing_fallback_puts_back_old_handlers()
		{
			int callCount = 0;
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				callCount++;
			}));
			SupportContainer newContainer = new SupportContainer ();
			viewManager.SetFallbackContainer(new object());
			viewManager.RemoveFallbackContainer ();
			newContainer.AddChild(new SupportView());
			Assert.That (callCount, Is.EqualTo (0));
		}

		[Test]
		public void adding_different_registry_fallback_removes_view_manager_fallback()
		{
			int callCount = 0;
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				callCount++;
			}));
			viewManager.SetFallbackContainer(new object());
			registry.SetFallbackContainer (new object ());
			container.AddChild(new SupportView());
			Assert.That (callCount, Is.EqualTo (0));
		}

		[Test]
		public void adding_another_fallback_in_registry_reverts_old_handlers()
		{
			int callCount = 0;
			viewManager.AddContainer(container);
			viewManager.AddViewHandler(new CallbackViewHandler(delegate(object view, Type type) {
				callCount++;
			}));
			viewManager.SetFallbackContainer(new object());
			registry.SetFallbackContainer (new object ());
			container.AddChild(new SupportView());
			Assert.That (callCount, Is.EqualTo (1));
		}
	}
}


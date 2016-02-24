//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
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
	[TestFixture]
	public class ContainerBindingTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private SupportContainer container;

		private ViewManager viewManager;

		private ContainerRegistry registry;

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

		[Test, ExpectedException]
		public void addContainer_throws_if_containers_are_nested()
		{
			SupportContainer container1 = new SupportContainer();
			SupportContainer container2 = new SupportContainer();
			container1.AddChild(container2);
			viewManager.AddContainer(container1);
			viewManager.AddContainer(container2);
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
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void handlers_are_called()
		{
			List<object> expected = new List<object>{"handler1", "handler2", "handler3"};
			List<object> actual = new List<object> ();
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
			container.AddChild(new SupportView());
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
			container.AddChild(new SupportView());
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
	}
}


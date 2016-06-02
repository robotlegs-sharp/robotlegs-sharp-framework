//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Extensions.ViewManagement.Support;
using NUnit.Framework;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Extensions.ContextViews;
using Robotlegs.Bender.Extensions.ContextViews.Impl;
using Robotlegs.Bender.Framework.Impl.LoggingSupport;
using Robotlegs.Bender.Extensions.ContextViews.API;

namespace Robotlegs.Bender.Extensions.ContextViews
{
	[TestFixture]
	public class ContextViewExtensionTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext context;

		private SupportContainer view;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			context = new Context();
			view = new SupportView();
		}

        /*============================================================================*/
        /* Tests                                                                      */
        /*============================================================================*/

        [Test]
        public void installing_after_initialization_throws_error()
        {
            Assert.Throws(typeof(LifecycleException), new TestDelegate(() =>
            {
                context.Initialize();
                context.Install(typeof(ContextViewExtension));
            }
            ));
        }

		[Test]
		public void icontextView_is_mapped()
		{
			IContextView actual = null;
			context.Install<ContextViewExtension>().Configure(new ContextView(view));
			context.WhenInitializing((Action)delegate() {
				actual = context.injector.GetInstance<IContextView>();
			});
			context.Initialize();
			Assert.That(actual.view, Is.EqualTo(view));
		}

		[Test]
		public void second_view_container_is_ignored()
		{
			IContextView actual = null;
			SupportView secondView = new SupportView();
			context.Install<ContextViewExtension>().Configure(new ContextView(view), new ContextView(secondView));
			context.WhenInitializing((Action)delegate() {
				actual = context.injector.GetInstance<IContextView>();
			});
			context.Initialize();
			Assert.That(actual.view, Is.EqualTo(view));
		}

		[Test]
		public void extension_logs_error_when_context_initialized_with_no_contextView()
		{
			bool errorLogged = false;
			CallbackLogTarget logTarget= new CallbackLogTarget(delegate(LogParams log) {
				if (log.source is ContextViewExtension && log.level == LogLevel.ERROR)
				{
					errorLogged = true;
				}
			});
			context.Install<ContextViewExtension>();
			context.AddLogTarget(logTarget);
			context.Initialize();
			Assert.That(errorLogged, Is.True);
		}
	}
}


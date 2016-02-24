//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Framework.API;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.ContextView;
using Robotlegs.Bender.Extensions.ContextView.Impl;
using Robotlegs.Bender.Extensions.ViewManagement.Support;

namespace Robotlegs.Bender.Extensions.ContextView
{
	public class StageSyncExtensionTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext context;

		private SupportView contextView;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			context = new Context();
			contextView = new SupportView();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Adding_ContextView_To_Stage_Initializes_Context()
		{
			context
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install(typeof(StageSyncExtension))
				.Configure(new ContextView.Impl.ContextView(contextView));
			contextView.AddThisView ();
			Assert.That (context.Initialized, Is.True);
		}

		[Test]
		public void Adding_ContextView_That_Is_Already_On_Stage_Initializes_Context()
		{
			contextView.AddThisView();
			context
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install(typeof(StageSyncExtension))
				.Configure(new ContextView.Impl.ContextView(contextView));
			Assert.That (context.Initialized, Is.True);
		}

		[Test]
		public void Removing_ContextView_From_Stage_Destroys_Context()
		{
			context
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install(typeof(StageSyncExtension))
				.Configure(new ContextView.Impl.ContextView(contextView));
			contextView.AddThisView ();
			contextView.RemoveThisView ();
			Assert.That (context.Destroyed, Is.True);
		}

		[Test]
		public void Installing_Stage_Sync_Extension_Before_View_State_Wacher_Waits_For_Watcher_Then_Initializes_Context()
		{
			context
				.Install(typeof(StageSyncExtension))
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Configure(new ContextView.Impl.ContextView(contextView));
			contextView.AddThisView ();
			Assert.That (context.Initialized, Is.True);
		}

		[Test, ExpectedException]
		public void Installing_Stage_Sync_Extension_Without_View_State_Wacher_Throws_Error()
		{
			context
				.Install(typeof(StageSyncExtension))
				.Configure(new ContextView.Impl.ContextView(contextView));
			contextView.AddThisView ();
			Assert.That (context.Initialized, Is.True);
		}
	}
}

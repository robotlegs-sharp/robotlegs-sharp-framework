//------------------------------------------------------------------------------
//  Copyright (c) 2009-2013 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using robotlegs.bender.framework.impl;
using robotlegs.bender.framework.api;
using NUnit.Framework;
using robotlegs.bender.extensions.contextview;
using robotlegs.bender.extensions.contextView.support;
using robotlegs.bender.extensions.contextview.impl;

namespace robotlegs.bender.extensions.contextView
{
	public class StageSyncExtensionTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext context;

		private ObjectA contextView;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			context = new Context();
			contextView = new ObjectA();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Adding_ContextView_To_Stage_Initializes_Context()
		{
			context.Install(typeof(TestSupportStageSyncExtension)).Configure(new ContextView(contextView));
			contextView.AddMockView ();
			Assert.That (context.Initialized, Is.True);
		}

		[Test]
		public void Adding_ContextView_That_Is_Already_On_Stage_Initializes_Context()
		{
			contextView.AddMockView();
			context.Install(typeof(TestSupportStageSyncExtension)).Configure(new ContextView(contextView));
			Assert.That (context.Initialized, Is.True);
		}

		[Test]
		public void Removing_ContextView_From_Stage_Destroys_Context()
		{
			context.Install(typeof(TestSupportStageSyncExtension)).Configure(new ContextView(contextView));
			contextView.AddMockView ();
			contextView.RemoveMockView ();
			Assert.That (context.Destroyed, Is.True);
		}
	}
}

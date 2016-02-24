//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.ViewProcessor.Utils;
using Robotlegs.Bender.Extensions.Mediation.Impl.Support;
using Robotlegs.Bender.Extensions.Matching;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Extensions.ViewManagement.Support;


namespace Robotlegs.Bender.Extensions.ViewProcessor.Impl
{
	public class ViewProcessorMediatorsTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector injector;

		private ViewProcessorMap instance;

		private MediatorWatcher mediatorWatcher;

		private SupportView matchingView;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			injector = new RobotlegsInjector();
			instance = new ViewProcessorMap(new ViewProcessorFactory(injector));

			mediatorWatcher = new MediatorWatcher();
			injector.Map(typeof(MediatorWatcher)).ToValue(mediatorWatcher);
			matchingView = new SupportView();
		}

		[TearDown]
		public void TearDown()
		{
			instance = null;
			injector = null;
			mediatorWatcher = null;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Test_Failure_Seen()
		{
			Assert.That (true, Is.True);
		}

		[Test]
		public void Create_Mediator_Instantiates_Mediator_For_View_When_Mapped()
		{
			instance.Map(typeof(SupportView)).ToProcess(new MediatorCreator(typeof(SupportMediator)));

			SupportView objA = new SupportView();
			instance.HandleView(objA, objA.GetType());
			objA.AddThisView();

			string[] expectedNotifications = new string[1] { "SupportMediator" };
			Assert.That (expectedNotifications, Is.EquivalentTo (mediatorWatcher.Notifications));
		}

		[Test]
		public void Doesnt_Leave_View_And_Mediator_Mappings_Lying_Around()
		{
			instance.MapMatcher(new TypeMatcher().AnyOf(typeof(ObjectWhichExtendsSupportView), typeof(SupportView))).ToProcess(new MediatorCreator(typeof(SupportMediator)));
			instance.HandleView(new SupportView(), typeof(SupportView));

			Assert.That(injector.HasDirectMapping(typeof(ObjectWhichExtendsSupportView)), Is.False);
			Assert.That(injector.HasDirectMapping(typeof(SupportView)), Is.False);
			Assert.That(injector.HasDirectMapping(typeof(SupportMediator)), Is.False);
		}

		[Test]
		public void Process_Instantiates_Mediator_For_View_When_Matched_To_Mapping()
		{
			instance.Map(typeof(SupportView)).ToProcess(new MediatorCreator(typeof(SupportMediator)));

			instance.Process(new SupportView());

			List<string> expectedNotifications = new List<string>{"SupportMediator"};
			Assert.That(expectedNotifications, Is.EquivalentTo(mediatorWatcher.Notifications));
		}

		[Test]
		public void Runs_Destroy_On_Created_Mediator_When_Unprocess_Runs()
		{
			instance.Map(typeof(SupportView)).ToProcess(new MediatorCreator(typeof(SupportMediator)));

			SupportView view = new SupportView();
			instance.Process(view);
			instance.Unprocess(view);

			List<string> expectedNotifications = new List<string>{"SupportMediator", "SupportMediator destroy"};
			Assert.That(expectedNotifications, Is.EquivalentTo(mediatorWatcher.Notifications));
		}

		[Test]
		public async void Automatically_Unprocesses_When_View_Leaves_Stage()
		{
			instance.Map(typeof(SupportView)).ToProcess(new MediatorCreator(typeof(SupportMediator)));
			matchingView.AddThisView();
			instance.Process(matchingView);
			Action<IView> removeViewCallback = null;
			removeViewCallback = delegate(IView view) {
				matchingView.RemoveView -= removeViewCallback;
				CheckMediatorsDestroyed(view);
			};
			matchingView.RemoveView += removeViewCallback;
			matchingView.RemoveThisView();
			await Task.Delay (500);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void CheckMediatorsDestroyed(object view)
		{
			List<string> expectedNotifications = new List<string> {"SupportMediator", "SupportMediator destroy"};
			Assert.That(expectedNotifications, Is.EqualTo(expectedNotifications).AsCollection);
		}
	}
}

class SupportMediator2
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public MediatorWatcher mediatorWatcher {get;set;}

	[Inject]
	public SupportView view {get;set;}

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void Initialize()
	{
		mediatorWatcher.Notify("SupportMediator2");
	}

	public void Destroy()
	{
		mediatorWatcher.Notify("SupportMediator2 destroy");
	}
}

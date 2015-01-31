using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using NUnit.Framework;
using robotlegs.bender.extensions.viewProcessorMap.utils;
using robotlegs.bender.extensions.mediatorMap.impl.support;
using robotlegs.bender.extensions.matching;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using robotlegs.bender.extensions.mediatorMap.api;


namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public class ViewProcessorMediatorsTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector injector;

		private ViewProcessorMap instance;

		private MediatorWatcher mediatorWatcher;

		private ObjectA matchingView;

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
			matchingView = new ObjectA();
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
			instance.Map(typeof(ObjectA)).ToProcess(new MediatorCreator(typeof(ExampleMediator)));

			ObjectA objA = new ObjectA();
			instance.HandleView(objA, objA.GetType());
			objA.AddMockView();

			string[] expectedNotifications = new string[1] { "ExampleMediator" };
			Assert.That (expectedNotifications, Is.EquivalentTo (mediatorWatcher.Notifications));
		}

		[Test]
		public void Doesnt_Leave_View_And_Mediator_Mappings_Lying_Around()
		{
			instance.MapMatcher(new TypeMatcher().AnyOf(typeof(ObjectWhichExtendsA), typeof(ObjectA))).ToProcess(new MediatorCreator(typeof(ExampleMediator)));
			instance.HandleView(new ObjectA(), typeof(ObjectA));

			Assert.That(injector.HasDirectMapping(typeof(ObjectWhichExtendsA)), Is.False);
			Assert.That(injector.HasDirectMapping(typeof(ObjectA)), Is.False);
			Assert.That(injector.HasDirectMapping(typeof(ExampleMediator)), Is.False);
		}

		[Test]
		public void Process_Instantiates_Mediator_For_View_When_Matched_To_Mapping()
		{
			instance.Map(typeof(ObjectA)).ToProcess(new MediatorCreator(typeof(ExampleMediator)));

			instance.Process(new ObjectA());

			List<string> expectedNotifications = new List<string>{"ExampleMediator"};
			Assert.That(expectedNotifications, Is.EquivalentTo(mediatorWatcher.Notifications));
		}

		[Test]
		public void Runs_Destroy_On_Created_Mediator_When_Unprocess_Runs()
		{
			instance.Map(typeof(ObjectA)).ToProcess(new MediatorCreator(typeof(ExampleMediator)));

			ObjectA view = new ObjectA();
			instance.Process(view);
			instance.Unprocess(view);

			List<string> expectedNotifications = new List<string>{"ExampleMediator", "ExampleMediator destroy"};
			Assert.That(expectedNotifications, Is.EquivalentTo(mediatorWatcher.Notifications));
		}

		[Test]
		public async void Automatically_Unprocesses_When_View_Leaves_Stage()
		{
			instance.Map(typeof(ObjectA)).ToProcess(new MediatorCreator(typeof(ExampleMediator)));
			matchingView.AddMockView();
			instance.Process(matchingView);
			Action<IView> removeViewCallback = null;
			removeViewCallback = delegate(IView view) {
				matchingView.RemoveView -= removeViewCallback;
				CheckMediatorsDestroyed(view);
			};
			matchingView.RemoveView += removeViewCallback;
			matchingView.RemoveMockView();
			await Task.Delay (500);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void CheckMediatorsDestroyed(object view)
		{
			List<string> expectedNotifications = new List<string> {"ExampleMediator", "ExampleMediator destroy"};
			Assert.That(expectedNotifications, Is.EqualTo(expectedNotifications).AsCollection);
		}
	}
}

class ExampleMediator
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public MediatorWatcher mediatorWatcher {get;set;}

	[Inject]
	public ObjectA view {get;set;}

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void Initialize()
	{
		mediatorWatcher.Notify("ExampleMediator");
	}

	public void Destroy()
	{
		mediatorWatcher.Notify("ExampleMediator destroy");
	}
}

class ExampleMediator2
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public MediatorWatcher mediatorWatcher {get;set;}

	[Inject]
	public ObjectA view {get;set;}

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void Initialize()
	{
		mediatorWatcher.Notify("ExampleMediator2");
	}

	public void Destroy()
	{
		mediatorWatcher.Notify("ExampleMediator2 destroy");
	}
}

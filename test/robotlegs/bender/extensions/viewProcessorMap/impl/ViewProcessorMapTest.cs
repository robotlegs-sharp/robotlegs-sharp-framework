using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.viewProcessorMap.support;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using NUnit.Framework;
using robotlegs.bender.extensions.viewManager.api;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using robotlegs.bender.extensions.mediatorMap.api;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public class ViewProcessorMapTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		// TODO: extract processing tests into own tests
		// TODO: add actual ViewProcessorMap tests

		private TypeMatcher objectAMatcher = new TypeMatcher().AllOf(typeof(ObjectA));

		private ViewProcessorMap viewProcessorMap;

		private TrackingProcessor trackingProcessor;

		private TrackingProcessor trackingProcessor2;

		private IInjector injector;

		private ObjectA matchingView;

		private ObjectB nonMatchingView;

		private GuardObject guardObject;

		private ObjectAWithWidthAndHeight matchingView2;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			injector = new RobotlegsInjector();
			injector.Map(typeof(RobotlegsInjector)).ToValue(injector);
			viewProcessorMap = new ViewProcessorMap(new ViewProcessorFactory(injector));
			trackingProcessor = new TrackingProcessor();
			trackingProcessor2 = new TrackingProcessor();
			matchingView = new ObjectA();
			nonMatchingView = new ObjectB();
			guardObject = new GuardObject();
			matchingView2 = new ObjectAWithWidthAndHeight();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Implements_IViewHandler()
		{
			Assert.That (viewProcessorMap, Is.InstanceOf (typeof(IViewHandler)));
		}

		[Test]
		public void Process_Passes_Mapped_Views_To_Processor_Instance_Process_With_Mapping_By_Type()
		{
			viewProcessorMap.Map(typeof(ObjectA)).ToProcess(trackingProcessor);
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.Process(nonMatchingView);

			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
		}

		[Test]
		public void Process_Passes_Mapped_Views_To_Processor_Instance_Process_With_Mapping_By_Matcher()
		{
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(trackingProcessor);
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.Process(nonMatchingView);
			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
		}

		[Test]
		public void Process_Passes_Mapped_Views_To_Processor_Class_Process_With_Mapping_By_Type()
		{
			viewProcessorMap.Map(typeof(ObjectA)).ToProcess(typeof(TrackingProcessor));
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.Process(nonMatchingView);
			AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor)), new object [1] { matchingView });
		}

		[Test]
		public void Process_Passes_Mapped_Views_To_Processor_Class_Process_With_Mapping_By_Matcher()
		{
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(typeof(TrackingProcessor));
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.Process(nonMatchingView);
			AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor)), new object[1] { matchingView });
		}

		[Test]
		public void Mapping_One_Matcher_To_Multiple_Processes_By_Class_All_Processes_Run()
		{
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(typeof(TrackingProcessor));
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(typeof(TrackingProcessor2));
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.Process(nonMatchingView);
			AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor)), new object[1] { matchingView});
			AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor2)), new object[1] { matchingView});
		}

		[Test]
		public void Mapping_One_Matcher_To_Multiple_Processes_By_Instance_All_Processes_Run()
		{
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(trackingProcessor);
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(trackingProcessor2);
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.Process(nonMatchingView);
			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
			AssertThatProcessorHasProcessedThese(trackingProcessor2, new object[1] { matchingView });
		}

		[Test]
		public void Duplicate_Identical_Mappings_By_Class_Do_Not_Repeat_Processes()
		{
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(typeof(TrackingProcessor));
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(typeof(TrackingProcessor));
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.Process(nonMatchingView);
			AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor)), new object[1] { matchingView });
		}

		[Test]
		public void Duplicate_Identical_Mappings_By_Instance_Do_Not_Repeat_Processes()
		{
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(trackingProcessor);
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(trackingProcessor);
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.Process(nonMatchingView);
			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
		}

		[Test]
		public void Unprocess_Passes_Mapped_Views_To_Processor_Instance_Unprocess_With_Mapping_By_Type()
		{
			viewProcessorMap.Map(typeof(ObjectA)).ToProcess(trackingProcessor);
			viewProcessorMap.Unprocess(matchingView);
			viewProcessorMap.Unprocess(nonMatchingView);
			AssertThatProcessorHasUnprocessedThese(trackingProcessor, new object[1] { matchingView });
		}

		[Test]
		public void Unprocess_Passes_Mapped_Views_To_Processor_Instance_Unprocess_With_Mapping_By_Matcher()
		{
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(trackingProcessor);
			viewProcessorMap.Unprocess(matchingView);
			viewProcessorMap.Unprocess(nonMatchingView);
			AssertThatProcessorHasUnprocessedThese(trackingProcessor, new object[1] { matchingView });
		}

		[Test]
		public void Unmapping_Matcher_From_Single_Processor_Stops_Further_Processing()
		{
			viewProcessorMap.MapMatcher(objectAMatcher).ToProcess(trackingProcessor);
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.UnmapMatcher(objectAMatcher).FromProcess(trackingProcessor);
			viewProcessorMap.Process(matchingView);
			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
		}

		[Test]
		public void Unmapping_Type_From_Single_Processor_Stops_Further_Processing()
		{
			viewProcessorMap.Map(typeof(ObjectA)).ToProcess(trackingProcessor);
			viewProcessorMap.Process(matchingView);
			viewProcessorMap.Unmap(typeof(ObjectA)).FromProcess(trackingProcessor);
			viewProcessorMap.Process(matchingView);
			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
		}

		[Test]
		public void Unmapping_From_Single_Processor_Keeps_Other_Processors_Intact()
		{
			viewProcessorMap.Map(typeof(ObjectA)).ToProcess(trackingProcessor);
			viewProcessorMap.Map(typeof(ObjectA)).ToProcess(trackingProcessor2);
			viewProcessorMap.Unmap(typeof(ObjectA)).FromProcess(trackingProcessor);
			viewProcessorMap.Process(matchingView);
			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[0]);
			AssertThatProcessorHasProcessedThese(trackingProcessor2, new object [1] { matchingView });
		}

		[Test]
		public void Unmapping_From_All_Processes_Removes_All_Processes()
		{
			viewProcessorMap.Map(matchingView.GetType()).ToProcess(typeof(TrackingProcessor));
			viewProcessorMap.Map(matchingView.GetType()).ToProcess(trackingProcessor2);
			viewProcessorMap.Unmap(matchingView.GetType()).FromAll();
			viewProcessorMap.Process(matchingView);
			AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor)), new object[0]);
			AssertThatProcessorHasProcessedThese(trackingProcessor2, new object[0]);
		}

		[Test]
		public void HandleItem_Passes_Mapped_views_To_Processor_Instance_Process_With_Mapping_By_Type()
		{
			viewProcessorMap.Map(matchingView.GetType()).ToProcess(trackingProcessor);
			viewProcessorMap.HandleView(matchingView, matchingView.GetType());
			viewProcessorMap.HandleView(nonMatchingView, nonMatchingView.GetType());
			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
		}

		[Test]
		public void A_Hook_Runs_And_Receives_Injection_Of_View()
		{
			viewProcessorMap.Map(matchingView2.GetType()).ToProcess(trackingProcessor).WithHooks(typeof(HookWithViewInjectionChangesSize));

			int expectedViewWidth = 100;
			int expectedViewHeight = 200;

			injector.Map(typeof(int), "rectHeight").ToValue(expectedViewHeight);
			injector.Map(typeof(int), "rectWidth").ToValue(expectedViewWidth);

			viewProcessorMap.Process(matchingView2);

			Assert.That(matchingView2.Width, Is.EqualTo(expectedViewWidth));
			Assert.That(matchingView2.Height, Is.EqualTo(expectedViewHeight));
		}

		[Test]
		public void Does_Not_Leave_View_Mapping_Lying_Around()
		{
			viewProcessorMap.Map(matchingView.GetType()).ToProcess(trackingProcessor);
			viewProcessorMap.HandleView(matchingView, matchingView.GetType());
			Assert.That(injector.HasMapping(matchingView.GetType()), Is.False);
		}

		[Test]
		public void Process_Runs_If_Guard_Allows_It()
		{
			viewProcessorMap.Map(guardObject.GetType()).ToProcess(trackingProcessor).WithGuards(typeof(OnlyIfViewApprovesGuard));
			guardObject.ShouldApprove = true;
			viewProcessorMap.Process(guardObject);
			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { guardObject });
		}

		[Test]
		public void Process_Does_Not_Run_If_Guard_Prevents_It()
		{
			viewProcessorMap.Map(guardObject.GetType()).ToProcess(trackingProcessor).WithGuards(typeof(OnlyIfViewApprovesGuard));
			viewProcessorMap.Process(guardObject);
			AssertThatProcessorHasProcessedThese(trackingProcessor, new object[0]);
		}

		[Test]
		public void Removing_A_Mapping_That_Does_Not_Exist_Does_Not_Throw_An_Error()
		{
			viewProcessorMap.Unmap(matchingView.GetType()).FromProcess(trackingProcessor);
		}

		[Test]
		public void Mapping_For_Injection_Results_In_View_Being_Injected()
		{
			string expectedInjectionValue = "Injected string";
			injector.Map(typeof(string)).ToValue(expectedInjectionValue);

			viewProcessorMap.Map(typeof(ViewNeedingInjection)).ToInjection();
			ViewNeedingInjection viewNeedingInjection = new ViewNeedingInjection();
			viewProcessorMap.Process(viewNeedingInjection);
			Assert.That(viewNeedingInjection.injectedValue, Is.EqualTo(expectedInjectionValue));
		}

		[Test]
		public void Unmapping_For_Injection_Results_In_View_Not_Being_Injected()
		{
			viewProcessorMap.Map(typeof(ViewNeedingInjection)).ToInjection();
			viewProcessorMap.Unmap(typeof(ViewNeedingInjection)).FromInjection();
			ViewNeedingInjection viewNeedingInjection = new ViewNeedingInjection();
			viewProcessorMap.Process(viewNeedingInjection);
			Assert.That(viewNeedingInjection.injectedValue, Is.EqualTo(null));
		}

		[Test]
		public void Mapping_To_No_Process_Still_Applies_Hooks()
		{
			viewProcessorMap.Map(matchingView2.GetType()).ToNoProcess().WithHooks(typeof(HookWithViewInjectionChangesSize));

			int expectedViewWidth = 100;
			int expectedViewHeight = 200;

			injector.Map(typeof(int), "rectHeight").ToValue(expectedViewHeight);
			injector.Map(typeof(int), "rectWidth").ToValue(expectedViewWidth);

			viewProcessorMap.Process(matchingView2);

			Assert.That(matchingView2.Width, Is.EqualTo(expectedViewWidth));
			Assert.That(matchingView2.Height, Is.EqualTo(expectedViewHeight));
		}

		[Test]
		public void Unmapping_From_No_Process_Does_Not_Apply_Hooks()
		{
			viewProcessorMap.Map(matchingView2.GetType()).ToNoProcess().WithHooks(typeof(HookWithViewInjectionChangesSize));

			injector.Map(typeof(int), "rectHeight").ToValue(100);
			injector.Map(typeof(int), "rectWidth").ToValue(200);

			viewProcessorMap.Unmap(matchingView2.GetType()).FromNoProcess();
			viewProcessorMap.Process(matchingView2);

			Assert.That(matchingView2.Width, Is.EqualTo(0));
			Assert.That(matchingView2.Height, Is.EqualTo(0));
		}

		[Test]
		public async void Automatically_Unprocesses_When_View_Leaves_Stage()
		{
			viewProcessorMap.Map(matchingView.GetType()).ToProcess(trackingProcessor);
			matchingView.AddMockView();
			viewProcessorMap.Process(matchingView);

			matchingView.RemoveView += CheckUnprocessorsRan;
			matchingView.RemoveMockView();
			await Task.Delay(500);
		}

		[Test]
		public void Hooks_Run_Before_Process()
		{
			List<Type> timingTracker = new List<Type>();
			injector.Map(typeof(List<Type>), "timingTracker").ToValue(timingTracker);
			viewProcessorMap.Map(matchingView.GetType()).ToProcess(typeof(Processor)).WithHooks(typeof(HookA));
			viewProcessorMap.Process(matchingView);
			Assert.That(timingTracker, Is.EquivalentTo(new Object[2]{ typeof(HookA), typeof(Processor) }));
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected object FromInjector(Type type)
		{
			if (!injector.HasDirectMapping(type))
			{
				injector.Map(type).AsSingleton();
			}
			return injector.GetInstance(type);
		}

		protected void AssertThatProcessorHasProcessedThese(object processor, object[] expected)
		{
			PropertyInfo processedViewsProperty = processor.GetType ().GetProperty ("ProcessedViews");
			Assert.That (processedViewsProperty, Is.Not.Null, String.Format("Object {0} does not contain a property called ProcessedViews", processor));

			object processedViews = processedViewsProperty.GetValue (processor);
	
			Assert.That(processedViews, Is.EqualTo(expected).AsCollection);
		}

		protected void AssertThatProcessorHasUnprocessedThese(object processor, object[] expected)
		{
			AssertThatProcessorHasUnprocessedThese(processor as TrackingProcessor, expected);
		}

		protected void AssertThatProcessorHasUnprocessedThese(TrackingProcessor processor, object[] expected)
		{
			Assert.That(processor.UnprocessedViews, Is.EqualTo(expected).AsCollection);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void CheckUnprocessorsRan(IView view)
		{
			AssertThatProcessorHasUnprocessedThese(trackingProcessor, new object[1] { matchingView });
		}

	}
}

class ViewNeedingInjection : object
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public string injectedValue {get;set;}
}

class OnlyIfViewApprovesGuard
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public GuardObject view {get;set;}

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public bool Approve()
	{
		return view.ShouldApprove;
	}
}

class ObjectAWithWidthAndHeight : ObjectA
{
	public int Width;

	public int Height;
}

class HookWithViewInjectionChangesSize
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public ObjectAWithWidthAndHeight view {get;set;}

	[Inject("rectWidth")]
	public int rectWidth {get;set;}

	[Inject("rectHeight")]
	public int rectHeight {get;set;}

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void Hook()
	{
		view.Width = rectWidth;
		view.Height = rectHeight;
	}
}

class HookA
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject("timingTracker")]
	public List<Type> timingTracker {get;set;}

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void Hook()
	{
		timingTracker.Add(typeof(HookA));
	}
}


class GuardObject : Object
{
	public bool ShouldApprove;
}

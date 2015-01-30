using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.viewProcessorMap.support;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using NUnit.Framework;
using robotlegs.bender.extensions.viewManager.api;
using System;
using System.Collections.Generic;

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
		/*
		[Test]
		public function unmapping_matcher_from_single_processor_stops_further_processing():void
		{
			viewProcessorMap.mapMatcher(spriteMatcher).toProcess(trackingProcessor);
			viewProcessorMap.process(matchingView);
			viewProcessorMap.unmapMatcher(spriteMatcher).fromProcess(trackingProcessor);
			viewProcessorMap.process(matchingView);
			assertThatProcessorHasProcessedThese(trackingProcessor, [matchingView]);
		}

		[Test]
		public function unmapping_type_from_single_processor_stops_further_processing():void
		{
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor);
			viewProcessorMap.process(matchingView);
			viewProcessorMap.unmap(Sprite).fromProcess(trackingProcessor);
			viewProcessorMap.process(matchingView);
			assertThatProcessorHasProcessedThese(trackingProcessor, [matchingView]);
		}

		[Test]
		public function unmapping_from_single_processor_keeps_other_processors_intact():void
		{
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor);
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor2);
			viewProcessorMap.unmap(Sprite).fromProcess(trackingProcessor);
			viewProcessorMap.process(matchingView);
			assertThatProcessorHasProcessedThese(trackingProcessor, []);
			assertThatProcessorHasProcessedThese(trackingProcessor2, [matchingView]);
		}

		[Test]
		public function unmapping_from_all_processes_removes_all_processes():void
		{
			viewProcessorMap.map(Sprite).toProcess(TrackingProcessor);
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor2);
			viewProcessorMap.unmap(Sprite).fromAll();
			viewProcessorMap.process(matchingView);
			assertThatProcessorHasProcessedThese(fromInjector(TrackingProcessor), []);
			assertThatProcessorHasProcessedThese(trackingProcessor2, []);
		}

		[Test]
		public function handleItem_passes_mapped_views_to_processor_instance_process_with_mapping_by_type():void
		{
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor);
			viewProcessorMap.handleView(matchingView, Sprite);
			viewProcessorMap.handleView(nonMatchingView, Shape);
			assertThatProcessorHasProcessedThese(trackingProcessor, [matchingView]);
		}

		[Test]
		public function a_hook_runs_and_receives_injection_of_view():void
		{
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor).withHooks(HookWithViewInjectionDrawsRectangle);

			const expectedViewWidth:Number = 100;
			const expectedViewHeight:Number = 200;

			injector.map(Number, "rectHeight").toValue(expectedViewHeight);
			injector.map(Number, "rectWidth").toValue(expectedViewWidth);

			viewProcessorMap.process(matchingView);

			assertThat(matchingView.width, equalTo(expectedViewWidth));
			assertThat(matchingView.height, equalTo(expectedViewHeight));
		}

		[Test]
		public function does_not_leave_view_mapping_lying_around():void
		{
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor);
			viewProcessorMap.handleView(matchingView, Sprite);
			assertThat(injector.hasMapping(Sprite), isFalse());
		}

		[Test]
		public function process_runs_if_guard_allows_it():void
		{
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor).withGuards(OnlyIfViewHasChildrenGuard);
			matchingView.addChild(new Sprite());
			viewProcessorMap.process(matchingView);
			assertThatProcessorHasProcessedThese(trackingProcessor, [matchingView]);
		}

		[Test]
		public function process_does_not_run_if_guard_prevents_it():void
		{
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor).withGuards(OnlyIfViewHasChildrenGuard);
			viewProcessorMap.process(matchingView);
			assertThatProcessorHasProcessedThese(trackingProcessor, []);
		}

		[Test]
		public function removing_a_mapping_that_does_not_exist_does_not_throw_an_error():void
		{
			viewProcessorMap.unmap(Sprite).fromProcess(trackingProcessor);
		}

		[Test]
		public function mapping_for_injection_results_in_view_being_injected():void
		{
			const expectedInjectionValue:Sprite = new Sprite();

			injector.map(IEventDispatcher).toValue(expectedInjectionValue);

			viewProcessorMap.map(Sprite).toInjection();
			const viewNeedingInjection:ViewNeedingInjection = new ViewNeedingInjection();
			viewProcessorMap.process(viewNeedingInjection);
			assertThat(viewNeedingInjection.injectedValue, equalTo(expectedInjectionValue));
		}

		[Test]
		public function unmapping_for_injection_results_in_view_not_being_injected():void
		{
			viewProcessorMap.map(Sprite).toInjection();
			viewProcessorMap.unmap(Sprite).fromInjection();
			const viewNeedingInjection:ViewNeedingInjection = new ViewNeedingInjection();
			viewProcessorMap.process(viewNeedingInjection);
			assertThat(viewNeedingInjection.injectedValue, equalTo(null));
		}

		[Test]
		public function mapping_to_no_process_still_applies_hooks():void
		{
			viewProcessorMap.map(Sprite).toNoProcess().withHooks(HookWithViewInjectionDrawsRectangle);

			const expectedViewWidth:Number = 100;
			const expectedViewHeight:Number = 200;

			injector.map(Number, "rectHeight").toValue(expectedViewHeight);
			injector.map(Number, "rectWidth").toValue(expectedViewWidth);

			viewProcessorMap.process(matchingView);

			assertThat(matchingView.width, equalTo(expectedViewWidth));
			assertThat(matchingView.height, equalTo(expectedViewHeight));
		}

		[Test]
		public function unmapping_from_no_process_does_not_apply_hooks():void
		{
			viewProcessorMap.map(Sprite).toNoProcess().withHooks(HookWithViewInjectionDrawsRectangle);

			injector.map(Number, "rectHeight").toValue(100);
			injector.map(Number, "rectWidth").toValue(200);

			viewProcessorMap.unmap(Sprite).fromNoProcess();
			viewProcessorMap.process(matchingView);

			assertThat(matchingView.width, equalTo(0));
			assertThat(matchingView.height, equalTo(0));
		}

		[Test(async)]
		public function automatically_unprocesses_when_view_leaves_stage():void
		{
			viewProcessorMap.map(Sprite).toProcess(trackingProcessor);
			container.addChild(matchingView);
			viewProcessorMap.process(matchingView);
			var asyncHandler:Function = Async.asyncHandler(this, checkUnprocessorsRan, 500);
			matchingView.addEventListener(Event.REMOVED_FROM_STAGE, asyncHandler);
			container.removeChild(matchingView);
		}

		[Test]
		public function hooks_run_before_process():void
		{
			const timingTracker:Array = [];
			injector.map(Array, "timingTracker").toValue(timingTracker);
			viewProcessorMap.map(Sprite).toProcess(Processor).withHooks(HookA);
			viewProcessorMap.process(matchingView);
			assertThat(timingTracker, array([HookA, Processor]));
		}
		//*/

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
			AssertThatProcessorHasProcessedThese (processor as TrackingProcessor, expected);
		}

		protected void AssertThatProcessorHasProcessedThese(TrackingProcessor processor, object[] expected)
		{
			Assert.That(processor.ProcessedViews, Is.EqualTo(expected).AsCollection);
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
/*
		private function CheckUnprocessorsRan(object view)
		{
			AssertThatProcessorHasUnprocessedThese(trackingProcessor, new object[1] { matchingView });
		}
		//*/
	}
}

class ViewNeedingInjection : object
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public string injectedValue;
}

class OnlyIfViewApprovesGuard
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public GuardObject view;

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public bool Approve()
	{
		return view.ShouldApprove;
	}
}

class HookWithViewInjectionChangesSize
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public ObjectA view;

	[Inject("rectWidth")]
	public int rectWidth;

	[Inject("rectHeight")]
	public int rectHeight;

	public int rectWidthAfterHook;

	public int rectHeightAfterHook;

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void Hook()
	{
		rectWidthAfterHook = rectWidth;
		rectHeightAfterHook = rectHeight;
//		view.graphics.drawRect(0, 0, rectWidth, rectHeight);
	}
}

class HookA
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject("timingTracker")]
	public List<Type> timingTracker;

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

//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.Matching;
using Robotlegs.Bender.Extensions.ViewProcessor.Support;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Extensions.ViewManagement.Support;

namespace Robotlegs.Bender.Extensions.ViewProcessor.Impl
{
    public class ViewProcessorMapTest
    {
        /*============================================================================*/
        /* Private Properties                                                         */
        /*============================================================================*/

        // TODO: extract processing tests into own tests
        // TODO: add actual ViewProcessorMap tests

        #region Fields

        private GuardObject guardObject;
        private IInjector injector;
        private SupportView matchingView;
        private SupportViewWithWidthAndHeight matchingView2;
        private ObjectB nonMatchingView;
        private TypeMatcher supportViewMatcher = new TypeMatcher().AllOf(typeof(SupportView));

        private TrackingProcessor trackingProcessor;
        private TrackingProcessor trackingProcessor2;
        private ViewProcessorMap viewProcessorMap;

        #endregion Fields

        /*============================================================================*/
        /* Test Setup and Teardown                                                    */
        /*============================================================================*/

        #region Methods

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
        public async Task Automatically_Unprocesses_When_View_Leaves_Stage()
        {
            viewProcessorMap.Map(matchingView.GetType()).ToProcess(trackingProcessor);
            matchingView.AddThisView();
            viewProcessorMap.Process(matchingView);

            matchingView.RemoveView += CheckUnprocessorsRan;
            matchingView.RemoveThisView();
            await Task.Delay(500);
        }

        [Test]
        public void Does_Not_Leave_View_Mapping_Lying_Around()
        {
            viewProcessorMap.Map(matchingView.GetType()).ToProcess(trackingProcessor);
            viewProcessorMap.HandleView(matchingView, matchingView.GetType());
            Assert.That(injector.HasMapping(matchingView.GetType()), Is.False);
        }

        [Test]
        public void Duplicate_Identical_Mappings_By_Class_Do_Not_Repeat_Processes()
        {
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(typeof(TrackingProcessor));
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(typeof(TrackingProcessor));
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.Process(nonMatchingView);
            AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor)), new object[1] { matchingView });
        }

        [Test]
        public void Duplicate_Identical_Mappings_By_Instance_Do_Not_Repeat_Processes()
        {
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(trackingProcessor);
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(trackingProcessor);
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.Process(nonMatchingView);
            AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
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
        public void Hooks_Run_Before_Process()
        {
            List<Type> timingTracker = new List<Type>();
            injector.Map(typeof(List<Type>), "timingTracker").ToValue(timingTracker);
            viewProcessorMap.Map(matchingView.GetType()).ToProcess(typeof(Processor)).WithHooks(typeof(HookA));
            viewProcessorMap.Process(matchingView);
            Assert.That(timingTracker, Is.EquivalentTo(new Object[2] { typeof(HookA), typeof(Processor) }));
        }

        [Test]
        public void Implements_IViewHandler()
        {
            Assert.That(viewProcessorMap, Is.InstanceOf(typeof(IViewHandler)));
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
        public void Mapping_One_Matcher_To_Multiple_Processes_By_Class_All_Processes_Run()
        {
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(typeof(TrackingProcessor));
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(typeof(TrackingProcessor2));
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.Process(nonMatchingView);
            AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor)), new object[1] { matchingView });
            AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor2)), new object[1] { matchingView });
        }

        [Test]
        public void Mapping_One_Matcher_To_Multiple_Processes_By_Instance_All_Processes_Run()
        {
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(trackingProcessor);
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(trackingProcessor2);
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.Process(nonMatchingView);
            AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
            AssertThatProcessorHasProcessedThese(trackingProcessor2, new object[1] { matchingView });
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
        public void Process_Does_Not_Run_If_Guard_Prevents_It()
        {
            viewProcessorMap.Map(guardObject.GetType()).ToProcess(trackingProcessor).WithGuards(typeof(OnlyIfViewApprovesGuard));
            viewProcessorMap.Process(guardObject);
            AssertThatProcessorHasProcessedThese(trackingProcessor, new object[0]);
        }

        [Test]
        public void Process_Passes_Mapped_Views_To_Processor_Class_Process_With_Mapping_By_Matcher()
        {
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(typeof(TrackingProcessor));
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.Process(nonMatchingView);
            AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor)), new object[1] { matchingView });
        }

        [Test]
        public void Process_Passes_Mapped_Views_To_Processor_Class_Process_With_Mapping_By_Type()
        {
            viewProcessorMap.Map(typeof(SupportView)).ToProcess(typeof(TrackingProcessor));
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.Process(nonMatchingView);
            AssertThatProcessorHasProcessedThese(FromInjector(typeof(TrackingProcessor)), new object[1] { matchingView });
        }

        [Test]
        public void Process_Passes_Mapped_Views_To_Processor_Instance_Process_With_Mapping_By_Matcher()
        {
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(trackingProcessor);
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.Process(nonMatchingView);
            AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
        }

        [Test]
        public void Process_Passes_Mapped_Views_To_Processor_Instance_Process_With_Mapping_By_Type()
        {
            viewProcessorMap.Map(typeof(SupportView)).ToProcess(trackingProcessor);
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.Process(nonMatchingView);

            AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
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
        public void Removing_A_Mapping_That_Does_Not_Exist_Does_Not_Throw_An_Error()
        {
            viewProcessorMap.Unmap(matchingView.GetType()).FromProcess(trackingProcessor);
        }

        [SetUp]
        public void Setup()
        {
            injector = new RobotlegsInjector();
            injector.Map(typeof(RobotlegsInjector)).ToValue(injector);
            viewProcessorMap = new ViewProcessorMap(new ViewProcessorFactory(injector));
            trackingProcessor = new TrackingProcessor();
            trackingProcessor2 = new TrackingProcessor();
            matchingView = new SupportView();
            nonMatchingView = new ObjectB();
            guardObject = new GuardObject();
            matchingView2 = new SupportViewWithWidthAndHeight();
        }

        /*============================================================================*/
        /* Tests                                                                      */
        /*============================================================================*/

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
        public void Unmapping_From_Single_Processor_Keeps_Other_Processors_Intact()
        {
            viewProcessorMap.Map(typeof(SupportView)).ToProcess(trackingProcessor);
            viewProcessorMap.Map(typeof(SupportView)).ToProcess(trackingProcessor2);
            viewProcessorMap.Unmap(typeof(SupportView)).FromProcess(trackingProcessor);
            viewProcessorMap.Process(matchingView);
            AssertThatProcessorHasProcessedThese(trackingProcessor, new object[0]);
            AssertThatProcessorHasProcessedThese(trackingProcessor2, new object[1] { matchingView });
        }

        [Test]
        public void Unmapping_Matcher_From_Single_Processor_Stops_Further_Processing()
        {
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(trackingProcessor);
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.UnmapMatcher(supportViewMatcher).FromProcess(trackingProcessor);
            viewProcessorMap.Process(matchingView);
            AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
        }

        [Test]
        public void Unmapping_Type_From_Single_Processor_Stops_Further_Processing()
        {
            viewProcessorMap.Map(typeof(SupportView)).ToProcess(trackingProcessor);
            viewProcessorMap.Process(matchingView);
            viewProcessorMap.Unmap(typeof(SupportView)).FromProcess(trackingProcessor);
            viewProcessorMap.Process(matchingView);
            AssertThatProcessorHasProcessedThese(trackingProcessor, new object[1] { matchingView });
        }

        [Test]
        public void Unprocess_Passes_Mapped_Views_To_Processor_Instance_Unprocess_With_Mapping_By_Matcher()
        {
            viewProcessorMap.MapMatcher(supportViewMatcher).ToProcess(trackingProcessor);
            viewProcessorMap.Unprocess(matchingView);
            viewProcessorMap.Unprocess(nonMatchingView);
            AssertThatProcessorHasUnprocessedThese(trackingProcessor, new object[1] { matchingView });
        }

        [Test]
        public void Unprocess_Passes_Mapped_Views_To_Processor_Instance_Unprocess_With_Mapping_By_Type()
        {
            viewProcessorMap.Map(typeof(SupportView)).ToProcess(trackingProcessor);
            viewProcessorMap.Unprocess(matchingView);
            viewProcessorMap.Unprocess(nonMatchingView);
            AssertThatProcessorHasUnprocessedThese(trackingProcessor, new object[1] { matchingView });
        }

        /*============================================================================*/
        /* Protected Functions                                                        */
        /*============================================================================*/

        protected void AssertThatProcessorHasProcessedThese(object processor, object[] expected)
        {
            PropertyInfo processedViewsProperty = processor.GetType().GetProperty("ProcessedViews");
            Assert.That(processedViewsProperty, Is.Not.Null, String.Format("Object {0} does not contain a property called ProcessedViews", processor));

            object processedViews = processedViewsProperty.GetValue(processor);

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

        protected object FromInjector(Type type)
        {
            if (!injector.HasDirectMapping(type))
            {
                injector.Map(type).AsSingleton();
            }
            return injector.GetInstance(type);
        }

        /*============================================================================*/
        /* Private Functions                                                          */
        /*============================================================================*/

        private void CheckUnprocessorsRan(IView view)
        {
            AssertThatProcessorHasUnprocessedThese(trackingProcessor, new object[1] { matchingView });
        }

        #endregion Methods
    }
}

internal class GuardObject : Object
{
    #region Fields

    public bool ShouldApprove;

    #endregion Fields
}

internal class HookA
{
    /*============================================================================*/
    /* Public Properties                                                          */
    /*============================================================================*/


    #region Properties

    [Inject("timingTracker")]
    public List<Type> timingTracker
    {
        get; set;
    }

    #endregion Properties

    /*============================================================================*/
    /* Public Functions                                                           */
    /*============================================================================*/

    #region Methods

    public void Hook()
    {
        timingTracker.Add(typeof(HookA));
    }

    #endregion Methods
}

internal class HookWithViewInjectionChangesSize
{
    /*============================================================================*/
    /* Public Properties                                                          */
    /*============================================================================*/

    #region Properties

    [Inject("rectHeight")]
    public int rectHeight
    {
        get; set;
    }

    [Inject("rectWidth")]
    public int rectWidth
    {
        get; set;
    }

    [Inject]
    public SupportViewWithWidthAndHeight view
    {
        get; set;
    }

    #endregion Properties

    /*============================================================================*/
    /* Public Functions                                                           */
    /*============================================================================*/

    #region Methods

    public void Hook()
    {
        view.Width = rectWidth;
        view.Height = rectHeight;
    }

    #endregion Methods
}

internal class OnlyIfViewApprovesGuard
{
    /*============================================================================*/
    /* Public Properties                                                          */
    /*============================================================================*/


    #region Properties

    [Inject]
    public GuardObject view
    {
        get; set;
    }

    #endregion Properties

    /*============================================================================*/
    /* Public Functions                                                           */
    /*============================================================================*/

    #region Methods

    public bool Approve()
    {
        return view.ShouldApprove;
    }

    #endregion Methods
}

internal class SupportViewWithWidthAndHeight : SupportView
{
    #region Fields

    public int Height;
    public int Width;

    #endregion Fields
}

internal class ViewNeedingInjection : object
{
    /*============================================================================*/
    /* Public Properties                                                          */
    /*============================================================================*/


    #region Properties

    [Inject]
    public string injectedValue
    {
        get; set;
    }

    #endregion Properties
}
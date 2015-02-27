using Moq;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.viewManager.support;
using NUnit.Framework;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.mediatorMap.api;
using System;
using robotlegs.bender.extensions.matching;
using System.Collections.Generic;
using robotlegs.bender.extensions.mediatorMap.support;
using robotlegs.bender.extensions.mediatorMap.impl.support;


namespace robotlegs.bender.extensions.mediatorMap.impl
{

	public class MediatorManagerTest
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<MediatorFactory> factory;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector injector;

		private MediatorManager manager;

		private SupportView container;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			injector = new RobotlegsInjector();
			manager = new MediatorManager();
			container = new SupportView ();
			container.AddThisView();
		}

		[TearDown]
		public void TearDown()
		{
			container.RemoveThisView();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Mediator_Is_Removed_From_Factory_When_View_Leaves_Stage()
		{
			SupportView view = new SupportView();
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(CallbackMediator));
			object mediator = injector.InstantiateUnmapped(typeof(CallbackMediator));
			container.AddChild(view);
			manager.AddMediator(mediator, view, mapping);
			container.RemoveChild(view);
			
			Assert.That (true, Is.False);
			//assertThat(factory, received().method('removeMediators').args(view).once());
		}

		[Test]
		public void Mediator_Is_NOT_Removed_When_View_Leaves_Stage_When_AutoRemove_Is_False()
		{
			SupportView view = new SupportView();
			MediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1] {typeof(SupportView)}), typeof(CallbackMediator));
			mapping.AutoRemove(false);
			object mediator = injector.InstantiateUnmapped(typeof(CallbackMediator));
			container.AddChild(view);
			manager.AddMediator(mediator, view, mapping);
			container.RemoveChild(view);
			Assert.That (true, Is.False);
			//assertThat(factory, received().method("removeMediators").never());
		}

		[Test]
		public void Mediator_Lifecycle_Methods_Are_Invoked()
		{
			List<string> expected = new List<string> {
				"preInitialize", "initialize", "postInitialize",
				"preDestroy", "destroy", "postDestroy"};
			List<string> actual = new List<string>();
			foreach (string phase in expected)
			{
				Action<string> callback = delegate(string ph) {
					actual.Add (ph);
				};
				injector.Map(typeof(Action<string>), phase + "Callback").ToValue(callback);
			}
			SupportView item = new SupportView();
			object mediator = injector.InstantiateUnmapped(typeof(LifecycleReportingMediator));
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(LifecycleReportingMediator));
			manager.AddMediator(mediator, item, mapping);
			manager.RemoveMediator(mediator, item, mapping);
			
			Assert.That(actual, Is.EquivalentTo(expected));
		}

		[Test]
		public void Mediator_Is_Given_View()
		{
			SupportView expected = new SupportView();
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1] {typeof(SupportView)}), typeof(LifecycleReportingMediator));
			LifecycleReportingMediator mediator = injector.InstantiateUnmapped(typeof(LifecycleReportingMediator)) as LifecycleReportingMediator;
			manager.AddMediator(mediator, expected, mapping);

			Assert.That (mediator.view, Is.EqualTo (expected));
		}

		[Test]
		public void Mediator_For_UIComponent_Is_Only_Initialized_After_CreationComplete()
		{
					//TODO: Matt Completed this test
			/*
			const view:UIComponent = new UIComponent();
			const mapping:IMediatorMapping = new MediatorMapping(createTypeFilter([UIComponent]), LifecycleReportingMediator);
			const mediator:LifecycleReportingMediator = injector.instantiateUnmapped(LifecycleReportingMediator);

			// we have to stub this out otherwise the manager assumes
			// that the mediator has been removed in the background
			stub(factory).method('getMediator').anyArgs().returns(mediator);
			manager.addMediator(mediator, view, mapping);
			container.addChild(view);

			assertThat("mediator starts off uninitialized", mediator.initialized, isFalse());
			delayAssertion(function():void {
				assertThat("mediator is eventually initialized", mediator.initialized, isTrue());
			}, 100);
			//*/
		}

		[Test]
		public void Old_Mediator_For_Reparented_UIComponent_Is_Not_Initialized_After_CreationComplete()
		{
			/*
			const view:UIComponent = new UIComponent();
			const mapping:IMediatorMapping = new MediatorMapping(createTypeFilter([UIComponent]), LifecycleReportingMediator);
			const oldMediator:LifecycleReportingMediator = injector.instantiateUnmapped(LifecycleReportingMediator);
			const newMediator:LifecycleReportingMediator = injector.instantiateUnmapped(LifecycleReportingMediator);

			manager.addMediator(oldMediator, view, mapping);
			manager.removeMediator(oldMediator, view, mapping);

			stub(factory).method('getMediator').anyArgs().returns(newMediator);
			manager.addMediator(newMediator, view, mapping);
			container.addChild(view);

			delayAssertion(function():void {
				assertThat("old mediator does not initialize", oldMediator.initialized, isFalse());
			}, 100);
			//*/
		}

		[Test]
		public void Mediator_For_UIComponent_Is_Not_Destroyed_When_Removed_Before_Initialization()
		{
					/*
			const view:UIComponent = new UIComponent();
			const mapping:IMediatorMapping = new MediatorMapping(createTypeFilter([UIComponent]), LifecycleReportingMediator);
			const mediator:LifecycleReportingMediator = injector.instantiateUnmapped(LifecycleReportingMediator);

			manager.addMediator(mediator, view, mapping);
			manager.removeMediator(mediator, view, mapping);
			container.addChild(view);

			delayAssertion(function():void {
				assertThat("mediator is not destroyed", mediator.destroyed, isFalse());
			}, 100);
			//*/
		}

		[Test]
		public void Mediator_Is_Given_NonDisplayObject_View()
		{
			float expected = 1.5f;
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1] {typeof(float)}), typeof(LifecycleReportingMediator));
			LifecycleReportingMediator mediator = injector.InstantiateUnmapped (typeof(LifecycleReportingMediator)) as LifecycleReportingMediator;
			manager.AddMediator(mediator, expected, mapping);

			Assert.That (mediator.view, Is.EqualTo (expected));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void DelayAssertion(Action closure, float delay = 50)
		{
			//Async.delayCall(this, closure, delay);
		}

		private ITypeFilter CreateTypeFilter(Type[] allOf, Type[] anyOf = null, Type[] noneOf = null)
		{
			TypeMatcher matcher = new TypeMatcher();
			if (allOf != null)
				matcher.AllOf(allOf);
			if (anyOf != null)
				matcher.AnyOf(anyOf);
			if (noneOf != null)
				matcher.NoneOf(noneOf);

			return matcher.CreateTypeFilter();
		}
	}
}
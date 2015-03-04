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
using robotlegs.bender.extensions.mediatorMap.dsl;


namespace robotlegs.bender.extensions.mediatorMap.impl
{

	public class MediatorManagerTest
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<IMediatorFactory> factory;

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
			factory = new Mock<IMediatorFactory> ();
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
			int callCount = 0;
			object actualView = null;
			manager.ViewRemoved += (Action<object>)delegate (object eventView) {
				callCount++;
				actualView = eventView;
			};
			SupportView view = new SupportView();
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(CallbackMediator));
			object mediator = injector.InstantiateUnmapped(typeof(CallbackMediator));
			container.AddChild(view);
			manager.AddMediator(mediator, view, mapping);
			container.RemoveChild(view);

			Assert.That(callCount, Is.EqualTo(1));
			Assert.That(actualView, Is.EqualTo(view));
		}

		[Test]
		public void Mediator_Is_NOT_Removed_When_View_Leaves_Stage_When_AutoRemove_Is_False()
		{
			int callCount = 0;
			manager.ViewRemoved += (Action<object>)delegate (object eventView) {
				callCount++;
			};

			SupportView view = new SupportView();
			MediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1] {typeof(SupportView)}), typeof(CallbackMediator));
			mapping.AutoRemove(false);
			object mediator = injector.InstantiateUnmapped(typeof(CallbackMediator));
			container.AddChild(view);
			manager.AddMediator(mediator, view, mapping);
			container.RemoveChild(view);

			Assert.That (callCount, Is.EqualTo (0));
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
		public void Mediator_For_UIComponent_Is_Initialized()
		{
			SupportView view = new SupportView ();
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(LifecycleReportingMediator));
			LifecycleReportingMediator mediator = injector.InstantiateUnmapped(typeof(LifecycleReportingMediator)) as LifecycleReportingMediator;

			factory.Setup (f => f.GetMediator (It.IsAny<object> (), It.IsAny<IMediatorMapping>())).Returns(mediator);
			Assert.That (mediator.initialized, Is.False);
			manager.AddMediator(mediator, view, mapping);
			container.AddChild (view);

			Assert.That (mediator.initialized, Is.True);
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
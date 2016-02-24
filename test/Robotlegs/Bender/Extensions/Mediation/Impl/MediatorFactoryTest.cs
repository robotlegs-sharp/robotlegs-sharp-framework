//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Moq;
using Robotlegs.Bender.Framework.API;
using NUnit.Framework;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Extensions.ViewManagement.Support;
using Robotlegs.Bender.Extensions.Matching;
using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.Mediation.Support;
using Robotlegs.Bender.Framework.Impl.HookSupport;
using Robotlegs.Bender.Framework.Impl.GuardSupport;
using Robotlegs.Bender.Extensions.Mediation.DSL;


namespace Robotlegs.Bender.Extensions.Mediation.Impl
{
	public class MediatorFactoryTest
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<IMediatorManager> manager;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector injector;

		private MediatorFactory factory;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			injector = new RobotlegsInjector();
			factory = new MediatorFactory(injector);
			manager = new Mock<IMediatorManager> ();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Mediator_Is_Created()
		{
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter( new Type[1] {typeof(SupportView)} ), typeof(CallbackMediator));
			object mediator = factory.CreateMediators(new SupportView(), typeof(SupportView), new List<IMediatorMapping>{mapping})[0];

			Assert.That(mediator, Is.InstanceOf(typeof(CallbackMediator)));
		}

		[Test]
		public void Mediator_Is_Injected_Into()
		{
			int expected = 128;
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1] { typeof(SupportView) }), typeof(InjectedMediator));
			injector.Map(typeof(int)).ToValue(expected);
			InjectedMediator mediator = factory.CreateMediators (new SupportView (), typeof(SupportView), new List<IMediatorMapping> { mapping }) [0] as InjectedMediator;

			Assert.That(mediator.number, Is.EqualTo(expected));
		}

		[Test]
		public void mediatedItem_is_injected_as_exact_type_into_mediator()
		{
			SupportView expected = new SupportView();
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1]{ typeof(SupportView) }), typeof(ViewInjectedMediator));
			ViewInjectedMediator mediator = factory.CreateMediators(expected, typeof(SupportView), new List<IMediatorMapping> {mapping})[0] as ViewInjectedMediator;

			Assert.That(mediator.mediatedItem, Is.EqualTo(expected));
		}

		[Test]
		public void mediatedItem_is_injected_as_requested_type_into_mediator()
		{

			SupportView expected = new SupportView();
			IMediatorMapping mapping = new MediatorMapping(CreateTypeFilter(
					new Type[1]{ typeof(SupportContainer) }), 
					typeof(ViewInjectedMediator));

			ViewInjectedMediator mediator = factory.CreateMediators(
				expected, 
				typeof(SupportView), 
				new List<IMediatorMapping> {mapping})[0] as ViewInjectedMediator;

			Assert.That(mediator.mediatedItem, Is.EqualTo(expected));
		}

		[Test]
		public void hooks_are_called()
		{
			Assert.That (HookCallCount (typeof(CallbackHook), typeof(CallbackHook)), Is.EqualTo (2));
		}

		[Test]
		public void hook_receives_mediator_and_mediatedItem()
		{
			SupportView mediatedItem = new SupportView();
			object injectedMediator = null;
			object injectedView = null;
			injector.Map(typeof(Action<MediatorHook>), "callback").ToValue((Action<MediatorHook>)delegate(MediatorHook hook) {
				injectedMediator = hook.mediator;
				injectedView = hook.mediatedItem;
			});

			MediatorMapping mapping = new MediatorMapping(CreateTypeFilter(
				new Type[1]{ typeof(SupportView) }), 
				typeof(ViewInjectedMediator));

			mapping.WithHooks(typeof(MediatorHook));

			factory.CreateMediators(mediatedItem, typeof(SupportView), new List<IMediatorMapping> {mapping});

			Assert.That(injectedMediator, Is.InstanceOf<ViewInjectedMediator>());
			Assert.That(injectedView, Is.EqualTo(mediatedItem));
		}

		[Test]
		public void mediator_is_created_when_the_guard_allows()
		{
			Assert.That(MediatorsCreatedWithGuards(typeof(HappyGuard)), Is.EqualTo(1));
		}

		[Test]
		public void mediator_is_created_when_all_guards_allow()
		{
			Assert.That(MediatorsCreatedWithGuards(typeof(HappyGuard), typeof(HappyGuard)), Is.EqualTo(1));
		}

		[Test]
		public void mediator_is_not_created_when_the_guard_denies()
		{
			Assert.That(MediatorsCreatedWithGuards(typeof(GrumpyGuard)), Is.EqualTo(0));
		}

		[Test]
		public void mediator_is_not_created_when_any_guards_denies()
		{
			Assert.That(MediatorsCreatedWithGuards(typeof(HappyGuard), typeof(GrumpyGuard)), Is.EqualTo(0));
		}

		[Test]
		public void mediator_is_not_created_when_all_guards_deny()
		{
			Assert.That(MediatorsCreatedWithGuards(typeof(GrumpyGuard), typeof(GrumpyGuard)), Is.EqualTo(0));
		}

		[Test]
		public void same_mediators_are_returned_for_mappings_and_mediatedItem()
		{
			SupportView mediatedItem = new SupportView();
			MediatorMapping mapping1 =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(ViewInjectedMediator));
			MediatorMapping mapping2 =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportContainer)}), typeof(ViewInjectedAsRequestedMediator));
			List<object> mediators1 = factory.CreateMediators(mediatedItem, typeof(SupportView), new List<IMediatorMapping>{mapping1, mapping2});
			List<object> mediators2 = factory.CreateMediators(mediatedItem, typeof(SupportView), new List<IMediatorMapping>{mapping1, mapping2});
			Assert.That (mediators1, Is.EqualTo (mediators2).AsCollection);
		}

		[Test]
		public void expected_number_of_mediators_are_returned_for_mappings_and_mediatedItem()
		{
			SupportView mediatedItem = new SupportView();
			MediatorMapping mapping1 =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(ViewInjectedMediator));
			MediatorMapping mapping2 =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportContainer)}), typeof(ViewInjectedAsRequestedMediator));
			List<object> mediators = factory.CreateMediators(mediatedItem, typeof(SupportView), new List<IMediatorMapping> {mapping1, mapping2});
			Assert.That(mediators.Count, Is.EqualTo(2));
		}

		[Test]
		public void getMediator()
		{
			SupportView mediatedItem = new SupportView();
			IMediatorMapping mapping =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(CallbackMediator));

			factory.CreateMediators(mediatedItem, typeof(SupportView), new List<IMediatorMapping> {mapping});
			Assert.That (factory.GetMediator (mediatedItem, mapping), Is.Not.Null);
		}

		[Test]
		public void removeMediator() 
		{
			SupportView mediatedItem = new SupportView();
			IMediatorMapping mapping =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(CallbackMediator));

			factory.CreateMediators(mediatedItem, typeof(SupportView), new List<IMediatorMapping> {mapping});
			factory.RemoveMediators(mediatedItem);

			Assert.That (factory.GetMediator (mediatedItem, mapping), Is.Null);
		}

		[Test]
		public void creating_mediator_gives_mediator_to_mediator_manager()
		{
			SupportView mediatedItem = new SupportView();
			IMediatorMapping mapping =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(CallbackMediator));

			injector.Map<IMediatorManager> ().ToValue (manager.Object);
			factory = new MediatorFactory (injector);
			factory.CreateMediators(mediatedItem, typeof(SupportView), new List<IMediatorMapping> {mapping});
			factory.CreateMediators(mediatedItem, typeof(SupportView), new List<IMediatorMapping> {mapping});

			manager.Verify(_manager=>_manager.AddMediator(It.IsAny<CallbackMediator>(), It.Is<SupportView>(arg2=>arg2 == mediatedItem), It.Is<IMediatorMapping>(arg3=>arg3 == mapping)), Times.Once);
		}

		[Test]
		public void removeMediator_removes_mediator_from_manager()
		{
			SupportView mediatedItem = new SupportView();
			IMediatorMapping mapping =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(CallbackMediator));

			injector.Map<IMediatorManager> ().ToValue (manager.Object);
			factory = new MediatorFactory(injector);
			factory.CreateMediators(mediatedItem, typeof(SupportView), new List<IMediatorMapping> {mapping});
			factory.RemoveMediators(mediatedItem);
			factory.RemoveMediators(mediatedItem);

			manager.Verify (_manager => _manager.RemoveMediator (It.IsAny<CallbackMediator> (), It.Is<object> (arg2 => arg2 == mediatedItem), It.Is<IMediatorMapping> (arg3 => arg3 == mapping)), Times.Once);
		}

		[Test]
		public void removeAllMediators_removes_all_mediators_from_manager()
		{
			SupportView mediatedItem1 = new SupportView();
			SupportView mediatedItem2 = new SupportView();
			IMediatorMapping mapping1 =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(CallbackMediator));
			IMediatorMapping mapping2 =
				new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportContainer)}), typeof(ViewInjectedAsRequestedMediator));

			injector.Map<IMediatorManager> ().ToValue (manager.Object);
			factory = new MediatorFactory (injector);
			factory.CreateMediators (mediatedItem1, typeof(SupportView), new List<IMediatorMapping>{ mapping1, mapping2 });
			factory.CreateMediators (mediatedItem2, typeof(SupportView), new List<IMediatorMapping>{ mapping1, mapping2 });
			factory.RemoveAllMediators();

			manager.Verify(_manager=>_manager.RemoveMediator(
				It.IsAny<CallbackMediator>(),
				It.Is<object>(arg2=>arg2==mediatedItem1),
				It.Is<IMediatorMapping>(arg3=>arg3==mapping1)), Times.Once);

			manager.Verify(_manager=>_manager.RemoveMediator(
				It.IsAny<ViewInjectedAsRequestedMediator>(),
				It.Is<object>(arg2=>arg2==mediatedItem1),
				It.Is<IMediatorMapping>(arg3=>arg3==mapping2)), Times.Once);

			manager.Verify(_manager=>_manager.RemoveMediator(
				It.IsAny<CallbackMediator>(),
				It.Is<object>(arg2=>arg2==mediatedItem2),
				It.Is<IMediatorMapping>(arg3=>arg3==mapping1)), Times.Once);

			manager.Verify(_manager=>_manager.RemoveMediator(
				It.IsAny<ViewInjectedAsRequestedMediator>(),
				It.Is<object>(arg2=>arg2==mediatedItem2),
				It.Is<IMediatorMapping>(arg3=>arg3==mapping2)), Times.Once);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private int HookCallCount(params object[] hooks)
		{
			int hookCallCount = 0;
			Action hookCallback = delegate() {
				hookCallCount++;
			};
			injector.Map(typeof(Action), "hookCallback").ToValue(hookCallback);

			MediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(CallbackMediator));
			mapping.WithHooks(hooks);
			factory.CreateMediators(new SupportView(), typeof(SupportView), new List<IMediatorMapping>{mapping});
			return hookCallCount;
		}

		private int MediatorsCreatedWithGuards(params object[] guards)
		{
			MediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1]{typeof(SupportView)}), typeof(CallbackMediator));
			mapping.WithGuards(guards);
			List<object> mediators = factory.CreateMediators(new SupportView(), typeof(SupportView), new List<IMediatorMapping> {mapping});
			return mediators.Count;
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

	class InjectedMediator
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public int number;
	}
	
	class ViewInjectedMediator
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public SupportView mediatedItem;
	}

	class ViewInjectedAsRequestedMediator
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public SupportContainer mediatedItem;
	}

	class MediatorHook
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public SupportView mediatedItem;

		[Inject]
		public ViewInjectedMediator mediator;

		[Inject(true, "callback")]
		public Action<MediatorHook> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Hook()
		{
			if(callback != null)
			{
				callback(this);
			}
		}
	}
}
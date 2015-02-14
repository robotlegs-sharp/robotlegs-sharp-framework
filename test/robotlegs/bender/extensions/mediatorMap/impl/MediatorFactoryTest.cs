using Moq;
using robotlegs.bender.framework.api;
using NUnit.Framework;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.viewManager.support;
using robotlegs.bender.extensions.matching;
using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.mediatorMap.support;


namespace robotlegs.bender.extensions.mediatorMap.impl
{
	public class MediatorFactoryTest
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<MediatorManager> manager;

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
			const expected:Sprite = new Sprite();

			const mapping:MediatorMapping =
				new MediatorMapping(createTypeFilter([DisplayObject]), ViewInjectedAsRequestedMediator);

			const mediator:ViewInjectedAsRequestedMediator =
				factory.createMediators(expected, Sprite, [mapping])[0];

			assertThat(mediator.mediatedItem, equalTo(expected));
		}

		[Test]
		public void hooks_are_called()
		{
			assertThat(hookCallCount(CallbackHook, CallbackHook), equalTo(2));
		}

		[Test]
		public void hook_receives_mediator_and_mediatedItem()
		{
			const mediatedItem:Sprite = new Sprite();
			var injectedMediator:Object = null;
			var injectedView:Object = null;
			injector.map(Action<MediatorHook>, "callback".toValue(function(hook:MediatorHook) {
				injectedMediator = hook.mediator;
				injectedView = hook.mediatedItem;
			});
			const mapping:MediatorMapping =
				new MediatorMapping(createTypeFilter([Sprite]), ViewInjectedMediator);

			mapping.withHooks(MediatorHook);

			factory.createMediators(mediatedItem, Sprite, [mapping]);

			assertThat(injectedMediator, instanceOf(ViewInjectedMediator));
			assertThat(injectedView, equalTo(mediatedItem));
		}

		[Test]
		public void mediator_is_created_when_the_guard_allows()
		{
			assertThat(mediatorsCreatedWithGuards(HappyGuard), equalTo(1));
		}

		[Test]
		public void mediator_is_created_when_all_guards_allow()
		{
			assertThat(mediatorsCreatedWithGuards(HappyGuard, HappyGuard), equalTo(1));
		}

		[Test]
		public void mediator_is_not_created_when_the_guard_denies()
		{
			assertThat(mediatorsCreatedWithGuards(GrumpyGuard), equalTo(0));
		}

		[Test]
		public void mediator_is_not_created_when_any_guards_denies()
		{
			assertThat(mediatorsCreatedWithGuards(HappyGuard, GrumpyGuard), equalTo(0));
		}

		[Test]
		public void mediator_is_not_created_when_all_guards_deny()
		{
			assertThat(mediatorsCreatedWithGuards(GrumpyGuard, GrumpyGuard), equalTo(0));
		}

		[Test]
		public void same_mediators_are_returned_for_mappings_and_mediatedItem()
		{
			const mediatedItem:Sprite = new Sprite();
			const mapping1:MediatorMapping =
				new MediatorMapping(createTypeFilter([Sprite]), ViewInjectedMediator);
			const mapping2:MediatorMapping =
				new MediatorMapping(createTypeFilter([DisplayObject]), ViewInjectedAsRequestedMediator);
			const mediators1:Array = factory.createMediators(mediatedItem, Sprite, [mapping1, mapping2]);
			const mediators2:Array = factory.createMediators(mediatedItem, Sprite, [mapping1, mapping2]);
			assertEqualsVectorsIgnoringOrder(mediators1, mediators2);
		}

		[Test]
		public void expected_number_of_mediators_are_returned_for_mappings_and_mediatedItem()
		{
			const mediatedItem:Sprite = new Sprite();
			const mapping1:MediatorMapping =
				new MediatorMapping(createTypeFilter([Sprite]), ViewInjectedMediator);
			const mapping2:MediatorMapping =
				new MediatorMapping(createTypeFilter([DisplayObject]), ViewInjectedAsRequestedMediator);
			const mediators:Array = factory.createMediators(mediatedItem, Sprite, [mapping1, mapping2]);
			assertThat(mediators.length, equalTo(2));
		}

		[Test]
		public void getMediator()
		{
			const mediatedItem:Sprite = new Sprite();
			const mapping:IMediatorMapping = new MediatorMapping(createTypeFilter([Sprite]), CallbackMediator);
			factory.createMediators(mediatedItem, Sprite, [mapping]);
			assertThat(factory.getMediator(mediatedItem, mapping), notNullValue());
		}

		[Test]
		public void removeMediator()
		{
			const mediatedItem:Sprite = new Sprite();
			const mapping:IMediatorMapping = new MediatorMapping(createTypeFilter([Sprite]), CallbackMediator);
			factory.createMediators(mediatedItem, Sprite, [mapping]);
			factory.removeMediators(mediatedItem);
			assertThat(factory.getMediator(mediatedItem, mapping), nullValue());
		}

		[Test]
		public void creating_mediator_gives_mediator_to_mediator_manager()
		{
			const mediatedItem:Sprite = new Sprite();
			const mapping:IMediatorMapping = new MediatorMapping(createTypeFilter([Sprite]), CallbackMediator);
			factory = new MediatorFactory(injector, manager);
			factory.createMediators(mediatedItem, Sprite, [mapping]);
			factory.createMediators(mediatedItem, Sprite, [mapping]);
			assertThat(manager, received().method("addMediator")
				.args(instanceOf(CallbackMediator), mediatedItem, mapping).once());
		}

		[Test]
		public void removeMediator_removes_mediator_from_manager()
		{
			const mediatedItem:Sprite = new Sprite();
			const mapping:IMediatorMapping = new MediatorMapping(createTypeFilter([Sprite]), CallbackMediator);
			factory = new MediatorFactory(injector, manager);
			factory.createMediators(mediatedItem, Sprite, [mapping]);
			factory.removeMediators(mediatedItem);
			factory.removeMediators(mediatedItem);
			assertThat(manager, received().method("removeMediator")
				.args(instanceOf(CallbackMediator), mediatedItem, mapping).once());
		}

		[Test]
		public void removeAllMediators_removes_all_mediators_from_manager()
		{
			const mediatedItem1:Sprite = new Sprite();
			const mediatedItem2:Sprite = new Sprite();
			const mapping1:IMediatorMapping = new MediatorMapping(createTypeFilter([Sprite]), CallbackMediator);
			const mapping2:IMediatorMapping = new MediatorMapping(createTypeFilter([DisplayObject]), ViewInjectedAsRequestedMediator);

			factory = new MediatorFactory(injector, manager);
			factory.createMediators(mediatedItem1, Sprite, [mapping1, mapping2]);
			factory.createMediators(mediatedItem2, Sprite, [mapping1, mapping2]);
			factory.removeAllMediators();

			assertThat(manager, received().method("removeMediator")
				.args(instanceOf(CallbackMediator), mediatedItem1, mapping1).once());

			assertThat(manager, received().method("removeMediator")
				.args(instanceOf(ViewInjectedAsRequestedMediator), mediatedItem1, mapping2).once());

			assertThat(manager, received().method("removeMediator")
				.args(instanceOf(CallbackMediator), mediatedItem2, mapping1).once());

			assertThat(manager, received().method("removeMediator")
				.args(instanceOf(ViewInjectedAsRequestedMediator), mediatedItem2, mapping2).once());
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
		public SupportView mediatedItem;
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
		public Action<object> callback;

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
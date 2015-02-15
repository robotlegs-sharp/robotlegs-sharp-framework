using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.mediatorMap.impl;
using NUnit.Framework;
using System.Collections.Generic;
using robotlegs.bender.extensions.mediatorMap.impl.support;
using robotlegs.bender.extensions.viewManager.support;
using robotlegs.bender.framework.impl;

namespace robotlegs.bender.extensions.mediatorMap.impl
{

	public class MediatorMapTestPreloaded
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector injector;

		private MediatorMap mediatorMap;

		private MediatorWatcher mediatorWatcher;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void SetUp()
		{
			Context context = new Context();
			injector = context.injector;
			mediatorMap = new MediatorMap(context);
			mediatorWatcher = new MediatorWatcher();
			injector.Map(typeof(MediatorWatcher)).ToValue(mediatorWatcher);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/
		/*
		[Test]
		public void a_hook_runs_and_receives_injections_of_view_and_mediator()
		{
			mediatorMap.map(Sprite).toMediator(RectangleMediator).withHooks(HookWithMediatorAndViewInjectionDrawsRectangle);

			const view:Sprite = new Sprite();

			const expectedViewWidth:Number = 100;
			const expectedViewHeight:Number = 200;

			injector.map(Rectangle).toValue(new Rectangle(0, 0, expectedViewWidth, expectedViewHeight));

			mediatorMap.handleView(view, null);

			assertThat(expectedViewWidth, equalTo(view.width));
			assertThat(expectedViewHeight, equalTo(view.height));
		}

		[Test]
		public void can_be_instantiated()
		{
			assertThat(mediatorMap is MediatorMap, isTrue());
		}

		[Test]
		public void create_mediator_instantiates_mediator_for_view_when_mapped()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator);

			mediatorMap.handleView(new Sprite(), null);

			List<string> expectedNotifications = new List<string> {"ExampleMediator"} ;
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void doesnt_leave_view_and_mediator_mappings_lying_around()
		{
			mediatorMap.mapMatcher(new TypeMatcher().anyOf(MovieClip, Sprite)).toMediator(ExampleMediator);
			mediatorMap.handleView(new Sprite(), null);

			assertThat(injector.satisfiesDirectly(MovieClip), isFalse());
			assertThat(injector.satisfiesDirectly(Sprite), isFalse());
			assertThat(injector.satisfiesDirectly(ExampleMediator), isFalse());
		}

		[Test]
		public void handler_creates_mediator_for_view_mapped_by_matcher()
		{
			mediatorMap.mapMatcher(new TypeMatcher().allOf(DisplayObject)).toMediator(ExampleDisplayObjectMediator);

			mediatorMap.handleView(new Sprite(), null);

			List<string> expectedNotifications = new List<string>{"ExampleDisplayObjectMediator"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void handler_doesnt_create_mediator_for_wrong_view_mapped_by_matcher()
		{
			mediatorMap.mapMatcher(new TypeMatcher().allOf(MovieClip)).toMediator(ExampleDisplayObjectMediator);

			mediatorMap.handleView(new Sprite(), null);

			const expectedNotifications:Vector.<String> = new <String>[];
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void handler_instantiates_mediator_for_view_mapped_by_type()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator);

			mediatorMap.handleView(new Sprite(), null);

			List<string> expectedNotifications = new List<string>{"ExampleMediator"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void implements_IViewHandler()
		{
			assertThat(mediatorMap, instanceOf(IViewHandler));
		}

		[Test]
		public void mediate_instantiates_mediator_for_view_when_matched_to_mapping()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator);

			mediatorMap.mediate(new Sprite());

			List<string> expectedNotifications = new List<string>{"ExampleMediator"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void mediator_is_created_if_guard_allows_it()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator).withGuards(OnlyIfViewHasChildrenGuard);
			const view:Sprite = new Sprite();
			view.addChild(new Sprite());
			mediatorMap.mediate(view);

			List<string> expectedNotifications = new List<string>{"ExampleMediator"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void no_mediator_is_created_if_guard_prevents_it()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator).withGuards(OnlyIfViewHasChildrenGuard);
			const view:Sprite = new Sprite();
			mediatorMap.mediate(view);

			const expectedNotifications:Vector.<String> = new <String>[];
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void runs_destroy_on_created_mediator_when_unmediate_runs()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator);

			const view:Sprite = new Sprite();
			mediatorMap.mediate(view);
			mediatorMap.unmediate(view);

			List<string> expectedNotifications = new List<string>{"ExampleMediator", "ExampleMediator destroy"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void mediator_is_created_for_non_view_object()
		{
			mediatorMap.map(NotAView).toMediator(NotAViewMediator);
			const notAView:NotAView = new NotAView();
			mediatorMap.mediate(notAView);

			List<string> expectedNotifications = List<string>{"NotAViewMediator"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void non_view_object_injected_into_mediator_correctly()
		{
			mediatorMap.map(NotAView).toMediator(NotAViewMediator);
			const notAView:NotAView = new NotAView();
			mediatorMap.mediate(notAView);
			assertThat(notAView.mediatorName, equalTo("NotAViewMediator"));
		}

		[Test]
		public void mediator_is_destroyed_for_non_view_object()
		{
			mediatorMap.map(NotAView).toMediator(NotAViewMediator);
			const notAView:NotAView = new NotAView();
			mediatorMap.mediate(notAView);
			mediatorMap.unmediate(notAView);

			List<string> expectedNotifications = new List<string> {"NotAViewMediator", "NotAViewMediator destroy"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void unmediate_cleans_up_mediators()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator);

			const view:Sprite = new Sprite();

			mediatorMap.mediate(view);
			mediatorMap.unmediate(view);

			List<string> expectedNotifications = new List<string> {"ExampleMediator", "ExampleMediator destroy"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void multiple_mappings_per_matcher_create_mediators()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator);
			mediatorMap.map(Sprite).toMediator(ExampleMediator2);

			mediatorMap.mediate(new Sprite());
			List<string> expectedNotifications:Vector.<String> = new List<string> {"ExampleMediator", "ExampleMediator2"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void multiple_mappings_per_matcher_destroy_mediators()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator);
			mediatorMap.map(Sprite).toMediator(ExampleMediator2);

			const view:Sprite = new Sprite();

			mediatorMap.mediate(view);
			mediatorMap.unmediate(view);

			const expectedNotifications:Vector.<String> = new List<string> {"ExampleMediator", "ExampleMediator2", "ExampleMediator destroy", "ExampleMediator2 destroy"};
			
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void only_one_mediator_created_if_identical_mapping_duplicated()
		{
			mediatorMap.map(Sprite).toMediator(ExampleMediator).withGuards(HappyGuard).withHooks(Alpha50PercentHook);
			mediatorMap.map(Sprite).toMediator(ExampleMediator).withGuards(HappyGuard).withHooks(Alpha50PercentHook);

			mediatorMap.mediate(new Sprite());
							const expectedNotifications:Vector.<String> = new List<string> {"ExampleMediator"};
			assertEqualsVectorsIgnoringOrder(expectedNotifications, mediatorWatcher.notifications);
		}

		[Test]
		public void removing_a_mapping_that_doesnt_exist_doesnt_throw_an_error()
		{
			mediatorMap.unmap(Sprite).fromMediator(ExampleMediator);
		}
		//*/
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
	public SupportView view {get; set;}

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void initialize()
	{
		mediatorWatcher.Notify("ExampleMediator");
	}

	public void destroy()
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
	public MediatorWatcher mediatorWatcher;

	[Inject]
	public SupportView view;

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void initialize()
	{
		mediatorWatcher.Notify("ExampleMediator2");
	}

	public void destroy()
	{
		mediatorWatcher.Notify("ExampleMediator2 destroy");
	}
}

class ExampleDisplayObjectMediator
{

	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	[Inject]
	public MediatorWatcher mediatorWatcher;

	[Inject]
	public SupportView view;

	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void initialize()
	{
		mediatorWatcher.Notify("ExampleDisplayObjectMediator");
	}


	class RectangleMediator
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public Rectangle rectangle;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void initialize()
		{

		}

	}

	struct Rectangle
	{
		public int x;
		public int y;
		public int width;
		public int height;
	}

	class OnlyIfViewHasChildrenGuard
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public SupportView view;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public bool Approve()
		{
			return (view.NumChildren > 0);
		}
	}

	class HookWithMediatorAndViewInjectionDrawsRectangle
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public RectangleMediator mediator;

		[Inject]
		public SupportViewWithWidthAndHeight view;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void hook()
		{
			int requiredWidth = mediator.rectangle.width;
			int requiredHeight = mediator.rectangle.height;
			view.Width = requiredWidth;
			view.Height = requiredHeight;
		}
	}

	class Alpha50PercentHook
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public SupportView view;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void hook()
		{
			//view.alpha = 0.5;
		}
	}

	class HookA
	{

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void hook()
		{
		}
	}

	class NotAView
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public string mediatorName;
	}

	class NotAViewMediator
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public NotAView notAView;

		[Inject]
		public MediatorWatcher mediatorWatcher;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void initialize()
		{
			notAView.mediatorName = "NotAViewMediator";
			mediatorWatcher.Notify("NotAViewMediator");
		}

		public void destroy()
		{
			mediatorWatcher.Notify("NotAViewMediator destroy");
		}
	}
}
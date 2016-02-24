//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Extensions.Mediation.Impl;
using NUnit.Framework;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.Mediation.Impl.Support;
using Robotlegs.Bender.Extensions.ViewManagement.Support;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Extensions.Matching;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Framework.Impl.GuardSupport;

namespace Robotlegs.Bender.Extensions.Mediation.Impl
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

		[Test]
		public void a_hook_runs_and_receives_injections_of_view_and_mediator()
		{
			mediatorMap.Map(typeof(SupportViewWithWidthAndHeight)).ToMediator(typeof(RectangleMediator)).WithHooks(typeof(HookWithMediatorAndViewInjectionDrawsRectangle));

			SupportViewWithWidthAndHeight view = new SupportViewWithWidthAndHeight();

			int expectedViewWidth = 100;
			int expectedViewHeight = 200;

			injector.Map(typeof(Rectangle)).ToValue(new Rectangle(0, 0, expectedViewWidth, expectedViewHeight));

			mediatorMap.HandleView(view, view.GetType());

			Assert.That(view.Width, Is.EqualTo(expectedViewWidth));
			Assert.That(view.Height, Is.EqualTo(expectedViewHeight));
		}

		[Test]
		public void can_be_instantiated()
		{
			Assert.That(mediatorMap, Is.InstanceOf<MediatorMap>());
		}

		[Test]
		public void create_mediator_instantiates_mediator_for_view_when_mapped()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher));

			mediatorMap.HandleView(new SupportView(), typeof(SupportView));

			List<string> expectedNotifications = new List<string> {"ExampleMediatorWatcher"} ;
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void doesnt_leave_view_and_mediator_mappings_lying_around()
		{
			mediatorMap.MapMatcher(new TypeMatcher().AnyOf(typeof(SupportView), typeof(SupportView))).ToMediator(typeof(ExampleMediatorWatcher));
			mediatorMap.HandleView(new SupportView(), typeof(SupportView));

			Assert.That(injector.SatisfiesDirectly(typeof(SupportEventView)), Is.False);
			Assert.That(injector.SatisfiesDirectly(typeof(SupportView)), Is.False);
			Assert.That(injector.SatisfiesDirectly(typeof(ExampleMediatorWatcher)), Is.False);
		}

		[Test]
		public void handler_creates_mediator_for_view_mapped_by_matcher()
		{
			mediatorMap.MapMatcher(new TypeMatcher().AllOf(typeof(SupportContainer))).ToMediator(typeof(ExampleDisplayObjectMediator));

			mediatorMap.HandleView(new SupportView(), typeof(SupportView));

			List<string> expectedNotifications = new List<string>{"ExampleDisplayObjectMediator"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void handler_doesnt_create_mediator_for_wrong_view_mapped_by_matcher()
		{
			mediatorMap.MapMatcher(new TypeMatcher().AllOf(typeof(SupportEventView))).ToMediator(typeof(ExampleDisplayObjectMediator));

			mediatorMap.HandleView(new SupportView(), typeof(SupportView));

			List<string> expectedNotifications = new List<string>();
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void handler_instantiates_mediator_for_view_mapped_by_type()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher));

			mediatorMap.HandleView(new SupportView(), typeof(SupportView));

			List<string> expectedNotifications = new List<string>{"ExampleMediatorWatcher"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void implements_IViewHandler()
		{
			Assert.That(mediatorMap, Is.InstanceOf<IViewHandler>());
		}

		[Test]
		public void mediate_instantiates_mediator_for_view_when_matched_to_mapping()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher));

			mediatorMap.Mediate(new SupportView());

			List<string> expectedNotifications = new List<string>{"ExampleMediatorWatcher"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void mediator_is_created_if_guard_allows_it()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher)).WithGuards(typeof(OnlyIfViewHasChildrenGuard));
			SupportView view = new SupportView();
			view.AddChild(new SupportView());
			mediatorMap.Mediate(view);

			List<string> expectedNotifications = new List<string>{"ExampleMediatorWatcher"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void no_mediator_is_created_if_guard_prevents_it()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher)).WithGuards(typeof(OnlyIfViewHasChildrenGuard));
			SupportView view = new SupportView();
			mediatorMap.Mediate(view);

			List<string> expectedNotifications = new List<string>();
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void runs_destroy_on_created_mediator_when_unmediate_runs()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher));

			SupportView view = new SupportView();
			mediatorMap.Mediate(view);
			mediatorMap.Unmediate(view);

			List<string> expectedNotifications = new List<string>{"ExampleMediatorWatcher", "ExampleMediatorWatcher destroy"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void mediator_is_created_for_non_view_object()
		{
			mediatorMap.Map(typeof(NotAView)).ToMediator(typeof(NotAViewMediator));
			NotAView notAView = new NotAView();
			mediatorMap.Mediate(notAView);

			List<string> expectedNotifications = new List<string>{"NotAViewMediator"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void non_view_object_injected_into_mediator_correctly()
		{
			mediatorMap.Map(typeof(NotAView)).ToMediator(typeof(NotAViewMediator));
			NotAView notAView = new NotAView();
			mediatorMap.Mediate(notAView);
			Assert.That(notAView.mediatorName, Is.EqualTo("NotAViewMediator"));
		}

		[Test] 
		public void mediator_is_destroyed_for_non_view_object()
		{
			mediatorMap.Map(typeof(NotAView)).ToMediator(typeof(NotAViewMediator));
			NotAView notAView = new NotAView();
			mediatorMap.Mediate(notAView);
			mediatorMap.Unmediate(notAView);

			List<string> expectedNotifications = new List<string> {"NotAViewMediator", "NotAViewMediator destroy"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void unmediate_cleans_up_mediators()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher));

			SupportView view = new SupportView();

			mediatorMap.Mediate(view);
			mediatorMap.Unmediate(view);

			List<string> expectedNotifications = new List<string> {"ExampleMediatorWatcher", "ExampleMediatorWatcher destroy"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void multiple_mappings_per_matcher_create_mediators()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher));
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher2));

			mediatorMap.Mediate(new SupportView());
			List<string> expectedNotifications = new List<string> {"ExampleMediatorWatcher", "ExampleMediatorWatcher2"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void multiple_mappings_per_matcher_destroy_mediators()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher));
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher2));

			SupportView view = new SupportView();

			mediatorMap.Mediate(view);
			mediatorMap.Unmediate(view);

			List<string> expectedNotifications = new List<string> {"ExampleMediatorWatcher", "ExampleMediatorWatcher2", "ExampleMediatorWatcher destroy", "ExampleMediatorWatcher2 destroy"};
			
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
		}

		[Test]
		public void only_one_mediator_created_if_identical_mapping_duplicated()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher)).WithGuards(typeof(HappyGuard)).WithHooks(typeof(AddChildHook));
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(ExampleMediatorWatcher)).WithGuards(typeof(HappyGuard)).WithHooks(typeof(AddChildHook));

			SupportView view = new SupportView ();
			mediatorMap.Mediate(view);
			List<string> expectedNotifications = new List<string> {"ExampleMediatorWatcher"};
			Assert.That (expectedNotifications, Is.EqualTo (mediatorWatcher.Notifications).AsCollection);
			Assert.That (view.NumChildren, Is.EqualTo (1));
		}

		[Test]
		public void removing_a_mapping_that_doesnt_exist_doesnt_throw_an_error()
		{
			mediatorMap.Unmap(typeof(SupportView)).FromMediator(typeof(ExampleMediatorWatcher));
		}
	}

	class ExampleMediatorWatcher
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

		public void Initialize()
		{
			mediatorWatcher.Notify("ExampleMediatorWatcher");
		}

		public void Destroy()
		{
			mediatorWatcher.Notify("ExampleMediatorWatcher destroy");
		}
	}

	class ExampleMediatorWatcher2
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

		public void Initialize()
		{
			mediatorWatcher.Notify("ExampleMediatorWatcher2");
		}

		public void Destroy()
		{
			mediatorWatcher.Notify("ExampleMediatorWatcher2 destroy");
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

		public void Initialize()
		{
			mediatorWatcher.Notify("ExampleDisplayObjectMediator");
		}
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

		public void Initialize()
		{

		}

	}

	public struct Rectangle
	{
		public int x;
		public int y;
		public int width;
		public int height;

		public Rectangle(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}
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

		public void Hook()
		{
			int requiredWidth = mediator.rectangle.width;
			int requiredHeight = mediator.rectangle.height;
			view.Width = requiredWidth;
			view.Height = requiredHeight;
		}
	}

	class AddChildHook
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public SupportView view;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Hook()
		{
			view.AddChild (new SupportView ());
		}
	}

	class HookA
	{

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Hook()
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

		public void Initialize()
		{
			notAView.mediatorName = "NotAViewMediator";
			mediatorWatcher.Notify("NotAViewMediator");
		}

		public void Destroy()
		{
			mediatorWatcher.Notify("NotAViewMediator destroy");
		}
	}
}
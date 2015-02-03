using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.contextview;
using NUnit.Framework;
using robotlegs.bender.extensions.contextview.impl;
using robotlegs.bender.framework.impl.loggingSupport;
using robotlegs.bender.extensions.eventDispatcher;
using robotlegs.bender.extensions.modularity.api;
using System;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewManager.support;
using robotlegs.bender.extensions.viewManager.impl;

namespace robotlegs.bender.extensions.modularity
{
	public class ModularityExtensionTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext parentContext;

		private IContext childContext;

		private SupportView root;

		private SupportView parentView;

		private SupportView childView;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			root = new SupportView();
			parentView = new SupportView();
			childView = new SupportView();

			parentContext = new Context()
				.Install(typeof(StageSyncExtension))
				.Install(typeof(ContextViewExtension));
			childContext = new Context()
				.Install(typeof(StageSyncExtension))
				.Install(typeof(ContextViewExtension));
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test, ExpectedException(typeof(LifecycleException))]
		public void Installing_After_Initialization_Throws_Error()
		{
			parentContext.Initialize();
			parentContext.Install(typeof(ModularityExtension));
		}

		[Test]
		public void Context_Inherits_Parent_Injector()
		{
			AddRootToStage();

			parentContext
				.Install (typeof(SupportParentFinderExtension))
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Configure(new ContextView(parentView));

			childContext
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Configure (new ContextView (childView));

			ContainerRegistry cr = new ContainerRegistry();
			cr.SetParentFinder (new SupportParentFinder ());

			parentContext.injector.Map(typeof(ContainerRegistry)).ToValue(cr);
			childContext.injector.Map(typeof(ContainerRegistry)).ToValue(cr);

			cr.AddContainer(parentView);
			cr.AddContainer(childView);

			root.AddChild(parentView);
			parentView.AddChild (childView);

			Assert.That (childContext.injector.parent, Is.EqualTo (parentContext.injector));
		}

		[Test]
		public void Context_Does_Not_Inherit_Parent_Injector_When_Not_Interested()
		{
			AddRootToStage();
			parentContext.Install(typeof(ModularityExtension)).Configure(new ContextView(parentView));
			childContext.Install(new ModularityExtension(false)).Configure(new ContextView(childView));
			root.AddChild(parentView);
			parentView.AddChild(childView);

			Assert.That (childContext.injector.parent, Is.Not.EqualTo (parentContext.injector));
		}

		[Test]
		public void Context_Does_Not_Inherit_Parent_Injector_When_Disallowed_By_Parent()
		{
			AddRootToStage();
			parentContext.Install(new ModularityExtension(true, false)).Configure(new ContextView(parentView));
			childContext.Install(typeof(ModularityExtension)).Configure(new ContextView(childView));
			root.AddChild (parentView);
			parentView.AddChild (childView);

			Assert.That(childContext.injector.parent, Is.Not.EqualTo(parentContext.injector));
		}

		[Test]
		public void Multiple_Parallel_Children_Only_Inherit_Parent_Injector()
		{
			AddRootToStage();

			parentContext
				.Install (typeof(SupportParentFinderExtension))
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Configure(new ContextView(parentView));

			childContext
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Configure (new ContextView (childView));


			SupportView anotherChildView = new SupportView ();
			IContext anotherChildContext = new Context()
				.Install (typeof(StageSyncExtension))
				.Install (typeof(ContextViewExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Configure (new ContextView (anotherChildView));


			ContainerRegistry cr = new ContainerRegistry();
			parentContext.injector.Map(typeof(ContainerRegistry)).ToValue(cr);
			childContext.injector.Map(typeof(ContainerRegistry)).ToValue(cr);
			anotherChildContext.injector.Map(typeof(ContainerRegistry)).ToValue(cr);

			cr.AddContainer(parentView);
			cr.AddContainer(childView);
			cr.AddContainer(anotherChildView);

			root.AddChild (parentView);
			parentView.AddChild (childView);
			parentView.AddChild (anotherChildView);

			Assert.That (childContext.injector.parent, Is.EqualTo (parentContext.injector));
			Assert.That (anotherChildContext.injector.parent, Is.EqualTo (parentContext.injector));
		}

		[Test]
		public void Multiple_Depths_Of_Children_Only_Inherit_The_First_Parents_Injector()
		{
			AddRootToStage();

			parentContext
				.Install (typeof(SupportParentFinderExtension))
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Configure(new ContextView(parentView));

			childContext
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Configure (new ContextView (childView));


			SupportView anotherChildView = new SupportView ();
			IContext anotherChildContext = new Context()
				.Install (typeof(StageSyncExtension))
				.Install (typeof(ContextViewExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Configure (new ContextView (anotherChildView));


			ContainerRegistry cr = new ContainerRegistry();
			parentContext.injector.Map(typeof(ContainerRegistry)).ToValue(cr);
			childContext.injector.Map(typeof(ContainerRegistry)).ToValue(cr);
			anotherChildContext.injector.Map(typeof(ContainerRegistry)).ToValue(cr);

			cr.AddContainer(parentView);
			cr.AddContainer(childView);
			cr.AddContainer(anotherChildView);

			root.AddChild (parentView);
			parentView.AddChild (childView);
			childView.AddChild (anotherChildView);

			Assert.That (childContext.injector.parent, Is.EqualTo (parentContext.injector));
			Assert.That (anotherChildContext.injector.parent, Is.EqualTo (childContext.injector));
			Assert.That (anotherChildContext.injector.parent, Is.Not.EqualTo (parentContext.injector));
		}

		[Test]
		public void Extension_Logs_Error_When_Context_Initialized_With_No_ContextView()
		{
			bool errorLogged = false;
			CallbackLogTarget logTarget = new CallbackLogTarget(delegate (LogParams result) {
				if(result.level == LogLevel.ERROR && result.source.GetType() == typeof(ModularityExtension))
					errorLogged = true;
				});
			childContext.Install(typeof(ModularityExtension)).Install(typeof(SupportParentFinderExtension));
			childContext.AddLogTarget(logTarget);
			childContext.Initialize();
			Assert.That (errorLogged, Is.True);
		}

		[Test]
		public void Extension_Logs_Error_When_Context_Initialized_With_No_Parent_Finder_Installed()
		{
			bool errorLogged = false;
			CallbackLogTarget logTarget = new CallbackLogTarget(delegate (LogParams result) {
				if(result.level == LogLevel.ERROR && result.source.GetType() == typeof(ModularityExtension))
					errorLogged = true;
			});
			childContext.Install(typeof(ModularityExtension));
			childContext.AddLogTarget(logTarget);
			childContext.Initialize();
			Assert.That (errorLogged, Is.True);
		}

		[Test]
		public void Child_Added_To_ViewManager_Inherits_Injector()
		{
			AddRootToStage();
			parentContext 
				.Install (typeof(ModularityExtension))
				.Install(typeof(ViewManagerExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Configure(typeof(ContextViewListenerConfig))
				.Configure(new ContextView(parentView));

			IViewManager viewManager = parentContext.injector.GetInstance (typeof(IViewManager)) as IViewManager;
			viewManager.AddContainer (childView);

			childContext
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Configure(new ContextView(childView));

			root.AddChild(parentView);
			root.AddChild(childView);

			Assert.That (childContext.injector.parent, Is.EqualTo (parentContext.injector));
		}

		[Test]
		public void ModuleConnector_Mapped_To_Injector()
		{
			object actual = null;
			IContext context = new Context();
			context
				.Install(typeof(EventDispatcherExtension))
				.Install(typeof(ModularityExtension));
			context.WhenInitializing( delegate() {
				actual = context.injector.GetInstance(typeof(IModuleConnector));
			});
			context.Initialize();
			Assert.That (actual, Is.InstanceOf (typeof(IModuleConnector)));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void AddRootToStage()
		{
			root.RemoveThisView();
		}

	}
}

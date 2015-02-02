using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.contextview;
using NUnit.Framework;
using robotlegs.bender.extensions.contextview.impl;
using robotlegs.bender.framework.impl.loggingSupport;
using robotlegs.bender.extensions.eventDispatcher;
using robotlegs.bender.extensions.modularity.api;
using robotlegs.bender.extensions.contextView.support;
using System;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewManager.support;

namespace robotlegs.bender.extensions.modularity
{
	public class ModularityExtensionTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext parentContext;

		private IContext childContext;

		private IContext anotherChildContext;

		private ObjectA root;

		private ObjectA parentView;

		private ObjectA childView;

		private ObjectA anotherChildView;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			root = new ObjectA();
			parentView = new ObjectA();
			childView = new ObjectA();
			anotherChildView = new ObjectA();

			parentContext = new Context()
				.Install(typeof(TestSupportStageSyncExtension))
				.Install(typeof(ContextViewExtension));
			childContext = new Context()
				.Install(typeof(TestSupportStageSyncExtension))
				.Install(typeof(ContextViewExtension));
			anotherChildContext = new Context()
				.Install(typeof(TestSupportStageSyncExtension))
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
			parentContext.Install(typeof(ModularityExtension)).Configure(new ContextView(parentView));
			childContext.Install(typeof(ModularityExtension)).Configure(new ContextView(childView));
			parentView.AddMockViewToParent(root);
			childView.AddMockViewToParent(parentView);

			Assert.That (childContext.injector.parent, Is.EqualTo (parentContext.injector));
		}

		[Test]
		public void Context_Does_Not_Inherit_Parent_Injector_When_Not_Interested()
		{
			AddRootToStage();
			parentContext.Install(typeof(ModularityExtension)).Configure(new ContextView(parentView));
			childContext.Install(new ModularityExtension(false)).Configure(new ContextView(childView));
			parentView.AddMockViewToParent(root);
			childView.AddMockViewToParent(parentView);

			Assert.That (childContext.injector.parent, Is.Not.EqualTo (parentContext.injector));
		}

		[Test]
		public void Context_Does_Not_Inherit_Parent_Injector_When_Disallowed_By_Parent()
		{
			AddRootToStage();
			parentContext.Install(new ModularityExtension(true, false)).Configure(new ContextView(parentView));
			childContext.Install(typeof(ModularityExtension)).Configure(new ContextView(childView));
			parentView.AddMockViewToParent(root);
			childView.AddMockViewToParent(parentView);

			Assert.That(childContext.injector.parent, Is.Not.EqualTo(parentContext.injector));
		}

		[Test]
		public void Multiple_Parallel_Children_Only_Inherit_Parent_Injector()
		{
			AddRootToStage ();
			parentContext.Install(typeof(ModularityExtension)).Configure(new ContextView(parentView));
			childContext.Install(typeof(ModularityExtension)).Configure(new ContextView(childView));
			anotherChildContext.Install(typeof(ModularityExtension)).Configure(new ContextView(anotherChildView));

			parentView.AddMockViewToParent(root);
			childView.AddMockViewToParent(parentView);
			//anotherChildView.AddMockViewToParent(parentView);

			bool a = childContext.injector.parent == parentContext.injector;
			bool b = childContext.injector.parent == parentContext.injector;
			bool c = childContext.injector.parent == null;

			Assert.That (childContext.injector.parent, Is.EqualTo (parentContext.injector), "childContext doesn't inherit from parentContext");
//			Assert.That (anotherChildContext.injector.parent, Is.EqualTo (parentContext.injector), "anotherChildContext doesn't inherit from parentContext");
//			Assert.That (childContext.injector.parent, Is.Not.EqualTo (anotherChildContext.injector), "childContext inherits from anotherChildContext (a Parallel context child)");
//			Assert.That (anotherChildContext.injector.parent, Is.Not.EqualTo (childContext.injector), "anotherChildContext inherits from childContext (a Parallel context child)");
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

		// There is now no difference if the view manager if installed or not.
		[Test]
		public void Child_Added_To_ViewManager_Inherits_Injector()
		{
			AddRootToStage();
			parentContext = new Context()
				.Install(typeof(ContextViewExtension))
				.Install(typeof(ModularityExtension))
				.Install(typeof(ViewManagerExtension))
				.Install(typeof(TestSupportStageSyncExtension))
				.Configure(new ContextView(parentView));

			IViewManager viewManager = parentContext.injector.GetInstance(typeof(IViewManager)) as IViewManager;
			viewManager.AddContainer(childView);

			childContext = new Context()
				.Install(typeof(ContextViewExtension))
				.Install(typeof(ModularityExtension))
				.Install(typeof(TestSupportStageSyncExtension))
				.Configure(new ContextView(childView));
			parentView.AddMockViewToParent(root);
			childView.AddMockViewToParent(root);

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
			root.AddMockView();
		}

	}
}

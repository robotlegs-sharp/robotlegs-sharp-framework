//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;
using Robotlegs.Bender.Extensions.ContextViews;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.ContextViews.Impl;
using Robotlegs.Bender.Framework.Impl.LoggingSupport;
using Robotlegs.Bender.Extensions.EventManagement;
using Robotlegs.Bender.Extensions.Modularity.API;
using System;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Extensions.ViewManagement;
using Robotlegs.Bender.Extensions.ViewManagement.Support;
using Robotlegs.Bender.Extensions.ViewManagement.Impl;
using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.Modularity.Impl;
using System.Reflection;

namespace Robotlegs.Bender.Extensions.Modularity
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

		[Test]
		public void Initialize_Context_Withought_Context_View_Thows_Error()
		{
			IContext context = new Context ();
			LogLevelTarget llt = new LogLevelTarget ();
			int errorCount = 0;
			context.AddLogTarget(llt);
			llt.ERROR += (LogLevelTarget.LogEventDelegate)delegate(object source, object message, object[] messageParams) {
				errorCount++;
				Assert.That(message, Contains.Substring("no contextview").IgnoreCase);
			};
			context.Install<ModularityExtension> ();
			context.Initialize ();
			Assert.That (errorCount, Is.EqualTo (1));
		}

		[Test]
		public void Initialize_Context_Should_Work()
		{
			IContext context = new Context ();
			context.Install<SupportParentFinderExtension> ();
			context.Install<TestSupportViewStateWatcherExtension> ();
			context.Install<ModularityExtension> ();
			LogLevelTarget llt = new LogLevelTarget ();
			context.AddLogTarget(llt);
			llt.ERROR += (LogLevelTarget.LogEventDelegate)delegate(object source, object message, object[] messageParams) {
				Assert.Fail();
			};
			context.Configure (new ContextView (root));
		}

		[Test]
		public void Install_ModularityExtension_Before_IViewStateWatcher_Will_Error()
		{
			IContext context = new Context ();
			context.Install<SupportParentFinderExtension> ();
			context.Install<ModularityExtension> ();
			context.Install<TestSupportViewStateWatcherExtension> ();
			LogLevelTarget llt = new LogLevelTarget ();
			context.AddLogTarget(llt);
			llt.ERROR += (LogLevelTarget.LogEventDelegate)delegate(object source, object message, object[] messageParams) {
				Console.WriteLine(message);
				Assert.That(message, Is.StringContaining("IViewStateWatcher installed prior to Modularity").IgnoreCase);
				Assert.Pass();
			};
			context.Configure (new ContextView (root));
			Assert.Fail();
		}

		[Test]
		public void Initialize_Context_Wihtout_IParentFinder_Logs_Error()
		{
			LogLevelTarget llt = new LogLevelTarget ();
			parentContext.AddLogTarget(llt);
			llt.ERROR += (LogLevelTarget.LogEventDelegate)delegate(object source, object message, object[] messageParams) {
				Assert.Pass();
			};
			parentContext.Install<TestSupportViewStateWatcherExtension> ();
			parentContext.Install<ModularityExtension> ();
			parentContext.Initialize ();
			Assert.Fail ();
		}

		[Test]
		public void Initialize_Context_Wihtout_IViewStateWatcher_Logs_Error()
		{
			LogLevelTarget llt = new LogLevelTarget ();
			parentContext.AddLogTarget(llt);
			llt.ERROR += (LogLevelTarget.LogEventDelegate)delegate(object source, object message, object[] messageParams) {
				Assert.Pass();
			};
			parentContext.Install<SupportParentFinderExtension> ();
			parentContext.Install<ModularityExtension> ();
			parentContext.Initialize ();
			Assert.Fail ();
		}

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
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
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
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Configure (new ContextView (childView));


			SupportView anotherChildView = new SupportView ();
			IContext anotherChildContext = new Context()
				.Install (typeof(StageSyncExtension))
				.Install (typeof(ContextViewExtension))
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
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
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Install (typeof(ModularityExtension))
				.Configure(new ContextView(parentView));

			childContext
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Configure (new ContextView (childView));


			SupportView anotherChildView = new SupportView ();
			IContext anotherChildContext = new Context()
				.Install (typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(StageSyncExtension))
				.Install (typeof(ContextViewExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
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
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install(typeof(ViewManagerExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Configure(typeof(ContextViewListenerConfig))
				.Configure(new ContextView(parentView));

			IViewManager viewManager = parentContext.injector.GetInstance (typeof(IViewManager)) as IViewManager;
			viewManager.AddContainer (childView);

			childContext
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Configure(new ContextView(childView));

			root.AddChild(parentView);
			root.AddChild(childView);

			Assert.That (childContext.injector.parent, Is.EqualTo (parentContext.injector));
		}

		[Test]
		public void ModuleConnector_Mapped_To_Injector()
		{
			object actual = null;
			IContext parentContext = new Context();
			parentContext
				.Install(typeof(EventDispatcherExtension))
				.Install(typeof(ModularityExtension));
			parentContext.WhenInitializing( delegate() {
				actual = parentContext.injector.GetInstance(typeof(IModuleConnector));
			});
			parentContext.Initialize();
			Assert.That (actual, Is.InstanceOf (typeof(IModuleConnector)));
		}

		[Test]
		public void Can_Get_Modularity_Dispatcher()
		{
			Assert.That (GetModularityEventDispatcher (), Is.Not.Null);
		}

		[Test]
		public void Modularity_Dispatcher_Should_Broadcast_When_Child_Context_Is_Added()
		{
			int eventFireCount = 0;

			AddRootToStage();
			root.AddChild(parentView);
			root.AddChild(childView);
			parentContext 
				.Install (typeof(EventDispatcherExtension))
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install(typeof(ViewManagerExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Configure(typeof(ContextViewListenerConfig))
				.Configure(new ContextView(parentView));

			GetModularityEventDispatcher().AddEventListener(ModularContextEvent.Type.CONTEXT_ADD, delegate() {
				eventFireCount++;
			});

			IViewManager viewManager = parentContext.injector.GetInstance (typeof(IViewManager)) as IViewManager;
			viewManager.AddContainer (childView);

			childContext
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Configure(new ContextView(childView));

			Assert.That (eventFireCount, Is.EqualTo(1));
		}

		[Test]
		public void Modularity_Dispatcher_Should_Broadcast_Add_Child_Before_Child_Context_Is_Initialized()
		{
			bool eventFired = false;
			bool childContextUninitialzied = false;

			AddRootToStage();
			root.AddChild(parentView);
			root.AddChild(childView);
			parentContext 
				.Install (typeof(EventDispatcherExtension))
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install(typeof(ViewManagerExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Configure(typeof(ContextViewListenerConfig))
				.Configure(new ContextView(parentView));

			GetModularityEventDispatcher().AddEventListener(ModularContextEvent.Type.CONTEXT_ADD, delegate() {
				eventFired = true;
				childContextUninitialzied = childContext.Uninitialized;
			});

			IViewManager viewManager = parentContext.injector.GetInstance (typeof(IViewManager)) as IViewManager;
			viewManager.AddContainer (childView);

			childContext
				.Install(typeof(TestSupportViewStateWatcherExtension))
				.Install (typeof(ModularityExtension))
				.Install (typeof(SupportParentFinderExtension))
				.Configure(new ContextView(childView));

			Assert.That (eventFired, Is.True);
			Assert.That (childContextUninitialzied, Is.True);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void AddRootToStage()
		{
			root.RemoveThisView();
		}

		private IEventDispatcher GetModularityEventDispatcher()
		{
			Type modularityType = typeof(ModularityExtension);
			FieldInfo fieldInfo = modularityType.GetField ("_modularityDispatcher", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
			return fieldInfo.GetValue (modularityType) as IEventDispatcher;
		}

	}
}

//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ContextViews.API;
using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using Robotlegs.Bender.Extensions.Modularity.API;
using Robotlegs.Bender.Extensions.Modularity.Impl;
using Robotlegs.Bender.Extensions.ViewManagement;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Extensions.Matching;

namespace Robotlegs.Bender.Extensions.Modularity
{
	/// <summary>
	/// <p>This extension allows a context to inherit dependencies from a parent context,
	/// and/or expose its dependencies to child contexts.</p>
	/// <p>It must be installed before context initialization.</p>
	/// </summary>
	public class ModularityExtension : IExtension
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext _context;

		private IInjector _injector;

		private ILogging _logger;

		private bool _inherit;

		private bool _expose;

		private object _contextView;

		private IViewStateWatcher _contextViewStateWatcher;

		private IParentFinder _parentFinder;

		private ViewManagerBasedExistenceWatcher _viewManagerBasedExistenceWatcher;

		private ContextViewBasedExistenceWatcher _contextViewBasedExistenceWatcher;

		private static IEventDispatcher _modularityDispatcher = new EventDispatcher();

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/// <summary>
		/// Modularity
		/// </summary>
		/// <param name="inherit">Should this context inherit dependencies from a parent context?</param>
		/// <param name="expose">Should this context expose its dependencies to child contexts?</param>
		public ModularityExtension(bool inherit = true, bool expose = true)
		{
			_inherit = inherit;
			_expose = expose;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend(IContext context)
		{
			context.BeforeInitializing (BeforeInitializing);
			_context = context;
			_injector = context.injector;
			_logger = context.GetLogger(this);
			_context.AddConfigHandler (new InstanceOfMatcher(typeof(IContextView)), HandleContextView);
			_injector.Map(typeof(IModuleConnector)).ToSingleton(typeof(ModuleConnector));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void HandleContextView(object contextViewObject)
		{
			IContextView contextView = contextViewObject as IContextView;
			_contextView = contextView.view;

			if (!_injector.HasDirectMapping (typeof(IViewStateWatcher)))
			{
				_logger.Error ("No IViewStateWatcher installed prior to Modularity Extension. The Modularity extension requires IViewStateWatcher to be installed first");
				return;
			}
			_contextViewStateWatcher = _injector.GetInstance (typeof(IViewStateWatcher)) as IViewStateWatcher;

			if (!_injector.HasDirectMapping (typeof(IParentFinder)))
			{
				_logger.Error ("No IParentFinder Installed. The Modularity extension required this");
				return;
			}
			_parentFinder = _injector.GetInstance (typeof(IParentFinder)) as IParentFinder;

			if (_expose)
			{
				ConfigureExistenceWatcher();
			}
			if (_inherit)
			{
				ConfigureExistenceBroadcaster();
			}
		}

		private void BeforeInitializing()
		{
			if (_contextView == null)
			{
				_logger.Error ("Context has no ContextView, and Modularity Extension doesn't allow this.");
				return;
			}
		}

		private void WhenDestroying()
		{
			if (_contextViewStateWatcher != null)
			{
				_contextViewStateWatcher.added -= HandleContextViewAdded;
			}
		}

		private void Init()
		{
			if (_expose)
			{
				ConfigureExistenceWatcher();
			}
			if (_inherit)
			{
				ConfigureExistenceBroadcaster();
			}
		}

		private void ConfigureExistenceWatcher()
		{
			if (_injector.HasDirectMapping (typeof(IViewManager)))
			{
				_logger.Debug("Context has a ViewManager. Configuring view manager based context existence watcher...");
				_viewManagerBasedExistenceWatcher = new ViewManagerBasedExistenceWatcher(_context, _contextView, _modularityDispatcher, _parentFinder, _injector.GetInstance(typeof(IViewManager)) as IViewManager);
				_viewManagerBasedExistenceWatcher.Init();
			}
			else
			{
				_logger.Debug ("Context has a ContextView. Configuring context view based context existence watcher...");
				_contextViewBasedExistenceWatcher = new ContextViewBasedExistenceWatcher (_context, _contextView, _modularityDispatcher, _parentFinder);
				_contextViewBasedExistenceWatcher.Init();
			}
		}

		private void ConfigureExistenceBroadcaster()
		{
			if (_contextViewStateWatcher.isAdded)
			{
				BroadcastContextExistence ();
			}
			else
			{
				_logger.Debug("Context view is not yet initialized. Waiting...");
				_contextViewStateWatcher.added += HandleContextViewAdded;
			}
		}

		private void HandleContextViewAdded (object contextView)
		{
			if (contextView == _contextView)
			{
				_contextViewStateWatcher.added -= HandleContextViewAdded;
				_logger.Debug("Context view is now added. Continuing...");
				BroadcastContextExistence ();
			}
		}

		private void BroadcastContextExistence()
		{
			_logger.Debug("Context configured to inherit. Broadcasting existence event...");
			_modularityDispatcher.Dispatch(new ModularContextEvent(ModularContextEvent.Type.CONTEXT_ADD, _context, _contextView));
		}
	}
}

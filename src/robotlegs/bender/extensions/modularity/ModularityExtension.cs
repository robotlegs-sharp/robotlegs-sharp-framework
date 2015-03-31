//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.contextview.api;
using robotlegs.bender.extensions.viewManager.api;
using System;
using robotlegs.bender.extensions.modularity.api;
using robotlegs.bender.extensions.modularity.impl;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.eventDispatcher.impl;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewManager.impl;

namespace robotlegs.bender.extensions.modularity
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

		private ILogger _logger;

		private bool _inherit;

		private bool _expose;

		private object _contextView;

		private IViewStateWatcher _contextViewStateWatcher;

		private IParentFinder _parentFinder;

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
			_context = context;
			_injector = context.injector;
			_logger = context.GetLogger(this);



			if (_injector.HasDirectMapping (typeof(IViewStateWatcher)))
			{
				_contextViewStateWatcher = _injector.GetMapping (typeof(IViewStateWatcher)) as IViewStateWatcher;
				Init ();
			}
			else
			{
				_context.AfterInitializing (BeforeInitializing);
			}

			_injector.Map(typeof(IModuleConnector)).ToSingleton(typeof(ModuleConnector));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void BeforeInitializing()
		{
			if (!_injector.HasDirectMapping (typeof(IContextView)))
			{
				_logger.Error("Context has no ContextView, and ModularityExtension doesn't allow this.");
				return;
			}
			_contextView = (_injector.GetInstance(typeof(IContextView)) as IContextView).view;

			if (!_injector.HasDirectMapping (typeof(IViewStateWatcher)))
			{
				_logger.Error ("No IViewStateWatcher Installed. The Modulation extension required this");
				return;
			}
			_contextViewStateWatcher = _injector.GetInstance (typeof(IViewStateWatcher)) as IViewStateWatcher;

			if (!_injector.HasDirectMapping (typeof(IParentFinder)))
			{
				_logger.Error ("No IParentFinder Installed. The Modulation extension required this");
				return;
			}
			_parentFinder = _injector.GetInstance (typeof(IParentFinder)) as IParentFinder;

			Init ();
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
				new ViewManagerBasedExistenceWatcher(_context, _contextView, _modularityDispatcher, _parentFinder, _injector.GetInstance(typeof(IViewManager)) as IViewManager).Init();
			}
			else
			{
				_logger.Debug ("Context has a ContextView. Configuring context view based context existence watcher...");
				new ContextViewBasedExistenceWatcher (_context, _contextView, _modularityDispatcher, _parentFinder).Init();
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
				_contextViewStateWatcher.added += HandleContextViewAdded;
			}
		}

		void HandleContextViewAdded (object contextView)
		{
			if (contextView == _contextView)
			{
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

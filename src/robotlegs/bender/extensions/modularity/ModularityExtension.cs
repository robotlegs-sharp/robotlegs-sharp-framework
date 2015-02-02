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
			context.BeforeInitializing(BeforeInitializingCheckForContextView);
			_context = context;
			_injector = context.injector;
			_logger = context.GetLogger(this);
			if (_injector.HasDirectMapping (typeof(IParentFinder)))
			{
				_parentFinder = _injector.GetInstance (typeof(IParentFinder)) as IParentFinder;
			}
			else
			{
				context.AddConfigHandler(new InstanceOfMatcher(typeof(IParentFinder)), HandleParentFinder);
			}
			context.AddConfigHandler(new InstanceOfMatcher(typeof(IContextView)), HandleContextView);
			_injector.Map(typeof(IModuleConnector)).ToSingleton(typeof(ModuleConnector));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void BeforeInitializingCheckForContextView()
		{
			if (_contextView == null)
			{
				_logger.Error("Context has no ContextView, and ModularityExtension doesn't allow this.");
			}
			if (_parentFinder == null)
			{
				_logger.Error("No IParentFinder installed, ModularityExtension required a skew of this extension.");
			}
		}

		private void HandleContextView(object contextView)
		{
			if (_contextView != null)
				return;
			IContextView castContextView = contextView as IContextView;
			if (castContextView == null)
				return;

			_contextView = castContextView.view;
			if (_parentFinder != null)
			{
				Init ();
			}
		}

		private void HandleParentFinder(object parentFinder)
		{
			if (_parentFinder != null)
				return;
			IParentFinder castParentFinder = _parentFinder as IParentFinder;
			if (castParentFinder == null)
				return;

			_parentFinder = castParentFinder;
			if (_contextView != null)
			{
				Init();
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
			_logger.Debug("Context has a ContextView. Configuring context view based context existence watcher...");
			new ContextViewBasedExistenceWatcher(_context, _contextView, _modularityDispatcher, _parentFinder);
		}

		private void ConfigureExistenceBroadcaster()
		{
			if(_context.Initialized)
			{
				BroadcastContextExistence();
			}
			else
			{
				_logger.Debug("Context view has not yet been added. Waiting...");
				_context.BeforeInitializing(BeforeContextInitializing);
			}
		}

		private void BeforeContextInitializing()
		{
			_logger.Debug("Context view is now added. Continuing...");
			BroadcastContextExistence();
		}

		private void BroadcastContextExistence()
		{
			_logger.Debug("Context configured to inherit. Broadcasting existence event...");
			_modularityDispatcher.Dispatch(new ModularContextEvent(ModularContextEvent.Type.CONTEXT_ADD, _context, _contextView));
		}
	}
}

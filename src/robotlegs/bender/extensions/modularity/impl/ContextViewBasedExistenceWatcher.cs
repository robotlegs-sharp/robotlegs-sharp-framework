using robotlegs.bender.framework.api;
using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.viewManager;

namespace robotlegs.bender.extensions.modularity.impl
{

	public class ContextViewBasedExistenceWatcher
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private ILogger _logger;

		private IContext _context;

		private object _contextView;

		private IEventDispatcher _modularityDispatcher;

		private IParentFinder _parentFinder;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ContextViewBasedExistenceWatcher(IContext context, object contextView, IEventDispatcher modularityDispatcher, IParentFinder parentFinder)
		{
			_logger = context.GetLogger(this);
			_context = context;
			_contextView = contextView;
			_modularityDispatcher = modularityDispatcher;
			_parentFinder = parentFinder;
			_context.WhenDestroying(Destroy);
			Init();
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void Init()
		{
			_logger.Debug("Listening for context existence events on Context {0}", _context);
			_modularityDispatcher.AddEventListener (ModularContextEvent.Type.CONTEXT_ADD, OnContextAdd);
		}

		private void Destroy()
		{
			_logger.Debug("Removing modular context existence event listener from context {0}", _context);
			_modularityDispatcher.RemoveEventListener (ModularContextEvent.Type.CONTEXT_ADD, OnContextAdd);
		}

		private void OnContextAdd(IEvent evt)
		{
			// TODO: Matt: Currently ALL ExistanceWatchers will hear ALL CONTEXT_ADD events.
			// We need to check if context is a child of _context as well as (context != _context)

			ModularContextEvent castEvent = evt as ModularContextEvent;
			object contextView = castEvent.ContextView;

			// We might catch out own existence event, so ignore that
			_parentFinder.Contains(_contextView, contextView);
			if (contextView != _contextView)
			{
				_logger.Debug("Context existence event caught. Configuring child context {0}", castEvent.Context);
				_context.AddChild(castEvent.Context);
			}
		}
	}
}

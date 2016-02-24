//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.ViewManagement;
using Robotlegs.Bender.Extensions.ViewManagement.Impl;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.Modularity.Impl
{

	public class ContextViewBasedExistenceWatcher
	{

		/*============================================================================*/
		/* protected Properties                                                       */
		/*============================================================================*/

		protected ILogging _logger;

		protected IContext _context;

		protected object _contextView;

		protected IEventDispatcher _modularityDispatcher;

		protected IParentFinder _parentFinder;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ContextViewBasedExistenceWatcher(IContext context, object contextView, IEventDispatcher modularityDispatcher, IParentFinder parentFinder)
		{
			_logger = context.GetLogger(this);
			_context = context;
			_contextView = contextView;
			_parentFinder = parentFinder;
			_modularityDispatcher = modularityDispatcher;
			_context.WhenDestroying(Destroy);
		}


		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public virtual void Init()
		{
			_logger.Debug("Listening for context existence events on Context {0}", _context);
			_modularityDispatcher.AddEventListener (ModularContextEvent.Type.CONTEXT_ADD, OnContextAdd);
		}

		/*============================================================================*/
		/* Protected Functions                                                          */
		/*============================================================================*/


		protected virtual void Destroy()
		{
			_logger.Debug("Removing modular context existence event listener from context {0}", _context);
			_modularityDispatcher.RemoveEventListener (ModularContextEvent.Type.CONTEXT_ADD, OnContextAdd);
		}

		protected virtual void OnContextAdd(IEvent evt)
		{
			ModularContextEvent castEvent = evt as ModularContextEvent;
			object contextView = castEvent.ContextView;

			// We might catch out own existence event, so ignore that
			if (contextView == _contextView)
				return;

			if (ValidateContextView (contextView))
			{
				_logger.Debug("Context existence event caught. Configuring child context {0}", castEvent.Context);
				_context.AddChild(castEvent.Context);
			}
		}

		protected virtual bool ValidateContextView(object contextView)
		{
			List<ContainerBinding> possibleParents = null;
			if (_parentFinder is ContainerRegistry)
			{
				possibleParents = (_parentFinder as ContainerRegistry).RootBindings;
			}

			return
				(possibleParents == null && _parentFinder.Contains (_contextView, contextView))
				|| (possibleParents != null && _parentFinder.FindParent (contextView, possibleParents) == _contextView);
		}

	}
}

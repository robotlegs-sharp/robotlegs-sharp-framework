//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ContextViews.API;
using Robotlegs.Bender.Extensions.Matching;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.ViewManagement
{
	public abstract class ViewStateWatcherExtension : IExtension
	{
		/*============================================================================*/
		/* Protected Properties                                                         */
		/*============================================================================*/

		protected ILogging _logger;

		protected IInjector _injector;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContextView _contextView;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			_injector = context.injector;
			_logger = context.GetLogger (this);

			if (context.injector.HasDirectMapping (typeof(IContextView)))
				HandleContextView (context.injector.GetInstance (typeof(IContextView)));
			else
				context.AddConfigHandler (new InstanceOfMatcher (typeof(IContextView)), HandleContextView);
		}

		/*============================================================================*/
		/* Private Functions                                                           */
		/*============================================================================*/

		private void HandleContextView(object contextView)
		{
			if (_contextView != null)
				return;
			IContextView castContextView = contextView as IContextView;
			if (castContextView == null)
				return;
			_contextView = castContextView;

			if (_injector.HasDirectMapping (typeof(IViewStateWatcher)))
				return;
			IViewStateWatcher viewStateWatcher = GetViewStateWatcher(_contextView.view);
			if (viewStateWatcher == null)
			{
				_logger.Warn ("Unable to create View State Watcher.");
				return;
			}
			_injector.Map(typeof(IViewStateWatcher)).ToValue(viewStateWatcher);
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected abstract IViewStateWatcher GetViewStateWatcher(object contextView);

	}
}


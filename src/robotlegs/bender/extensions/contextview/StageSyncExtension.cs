
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.contextview.api;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.contextview.impl;
using robotlegs.bender.extensions.contextviewstatewatcher;

namespace robotlegs.bender.extensions.contextview
{
	/**
	 * <p>This Extension will automatically Initialize the Context when the ContextView is added to the stage.
	 * If the ContextView is already added when this extension is installed it will run Initialize straight away.</p>
	 */
	public class StageSyncExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext _context;
		private IContextView _contextView;
		private ILogger _logger;
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			_context = context;
			_logger = context.GetLogger(this);

			if(context.injector.HasMapping(typeof(IContextView)))
				HandleContextView(context.injector.GetInstance(typeof(IContextView)));
			else
				context.AddConfigHandler(new AssignableFromMatcher(typeof(IContextView)), HandleContextView);
		}

		/*============================================================================*/
		/* Private Functions                                                           */
		/*============================================================================*/

		private void HandleContextView(object contextView)
		{
			if (_contextView != null)
			{
				_logger.Warn("A contextView has already been installed, ignoring {0}", contextView);
				return;
			}
			IContextView castContextView = contextView as IContextView;
			if(castContextView == null)
			{
				_logger.Warn("The object mapped to IContextView is not an IContextView. {0}", contextView);
				return;
			}

			_contextView = castContextView;

			// Matt to James? Can we add injection hook and listen for IContextViewStateWatcher to mapped?
				// If so we dont need the contextViewStateWatcherSet event in the contextview
			if(_contextView.contextViewStateWatcher == null)
			{
				_contextView.contextViewStateWatcherSet += HandleContextViewStateWatcherSet;
				return;
			}
			ContextViewStateWatcherSet();
		}

		private void HandleContextViewStateWatcherSet()
		{
			_contextView.contextViewStateWatcherSet -= HandleContextViewStateWatcherSet;
			ContextViewStateWatcherSet ();
		}

		private void ContextViewStateWatcherSet()
		{
			if(_contextView.contextViewStateWatcher.isAddedToStage)
			{
				InitializeContext();
			}
			else
			{
				_contextView.contextViewStateWatcher.addedToStage += HandleContextViewAddedToStage;
			}

		}

		private void HandleContextViewAddedToStage(object view)
		{
			_contextView.contextViewStateWatcher.addedToStage -= HandleContextViewAddedToStage;
			InitializeContext();
		}

		private void InitializeContext()
		{
			_contextView.contextViewStateWatcher.removeFromStage += HandleContextViewRemoveFromStage;
			_contextView.contextViewStateWatcher.suspended += HandleContextViewSuspended;
			_context.Initialize();
		}
/*
 * START QUESTIONNABLE REGION
 * Should the handling of suspend + resume via the Context view state watcher be managed from another extension?
*/
		private void HandleContextViewSuspended(object view)
		{
			_contextView.contextViewStateWatcher.suspended -= HandleContextViewSuspended;
			_contextView.contextViewStateWatcher.resumed += HandleContextViewResumed;
			_context.Suspend();
		}

		private void HandleContextViewResumed(object view)
		{
			_contextView.contextViewStateWatcher.suspended += HandleContextViewSuspended;
			_contextView.contextViewStateWatcher.resumed -= HandleContextViewResumed;
			_context.Resume();
		}		
/*
 * END  QUESTIONNABLE REGION
*/
		private void HandleContextViewRemoveFromStage(object view)
		{
			_contextView.contextViewStateWatcher.removeFromStage -= HandleContextViewRemoveFromStage;
			_context.Destroy();	
		}
	}
}


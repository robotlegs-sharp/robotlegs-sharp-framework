
using robotlegs.bender.extensions.contextviewstatewatcher;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.contextview.api;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.contextview.impl;

namespace robotlegs.bender.extensions.contextview
{	
	/// <summary>
	/// <p>This Extension class is abstract and should be extended to make a platform specific version.</p>
	/// <p>Installing this extension as it is will cause an error.</p>
	///
	/// <p>This extention should instantiate the ContextViewStateWatcher view in which ever way appropriate to your platform.
	/// In your extended class override the GetContextViewStateWatcher and return your ContextViewStateWatcher view (Making sure it implements IContextViewStateWatcher).</p>
	/// </summary>
	public abstract class ContextViewStateWatcherExtension : IExtension
	{
		protected IContext _context;
		protected IContextView _contextView;
		protected ILogger _logger;
		
		public virtual void Extend (IContext context)
		{
			_context = context;
			_logger = context.GetLogger(this);

			if(context.injector.HasMapping(typeof(IContextView)))
				HandleContextView(context.injector.GetInstance(typeof(IContextView)));
			else
				context.AddConfigHandler(new AssignableFromMatcher(typeof(IContextView)), HandleContextView);
		}
		
		protected virtual void HandleContextView(object contextView)
		{
			if (_contextView != null) return;
			IContextView castContextView = contextView as IContextView;
			if(castContextView == null) return;
			
			_contextView = castContextView;
			
			if(_contextView.contextViewStateWatcher != null)
			{
				_logger.Warn("A contextViewStateWatcher has already been installed");
				return;
			}

			IContextViewStateWatcherSetter contextViewStateWatcherSetter = castContextView as IContextViewStateWatcherSetter;
			if(contextViewStateWatcherSetter == null)
			{
				_logger.Warn("The ContextViewStateWatcher cannot be set as the context view doesn't implement IContextViewStateWatcherSetter", this);
				return;
			}

			IContextViewStateWatcher contextViewStateWatcher = GetContextViewStateWatcher(_contextView.view);
			if(contextViewStateWatcher == null)
			{
				_logger.Warn("The IContextViewStateWatcher cannot be created as GetContextViewStateWatcher returned null");
				return;
			}

			contextViewStateWatcherSetter.contextViewStateWatcher = contextViewStateWatcher;
			_context.injector.Map(typeof(IContextViewStateWatcher)).ToValue(contextViewStateWatcher);
		}

		protected abstract IContextViewStateWatcher GetContextViewStateWatcher (object contextView);
	}
}


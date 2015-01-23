using System;
using robotlegs.bender.extensions.contextview.api;

namespace robotlegs.bender.extensions.contextview.impl
{

	public class ContextView : IContextView, IContextViewStateWatcherSetter
	{
		private object _view;
		private IContextViewStateWatcher _contextViewStateWatcher;
		public event Action contextViewStateWatcherSet;

		public ContextView (object view) :this(view, null) {}

			// Not sure if we want this overload?
		public ContextView (object view, IContextViewStateWatcher contextViewStateWatcher)
		{
			_view = view;
			_contextViewStateWatcher = contextViewStateWatcher;
		}
		
		public object view
		{
			get
			{
				return _view;
			}
		}
		
		public IContextViewStateWatcher contextViewStateWatcher
		{
			get
			{
				return _contextViewStateWatcher;
			}
			set
			{
				_contextViewStateWatcher = value;
				if(contextViewStateWatcherSet != null) contextViewStateWatcherSet();
			}
		}
	}
}


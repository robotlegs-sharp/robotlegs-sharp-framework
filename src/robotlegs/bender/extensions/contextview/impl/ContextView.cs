using System;
using robotlegs.bender.extensions.contextview.api;

namespace robotlegs.bender.extensions.contextview.impl
{
	public class ContextView : IContextView
	{
		private object _view;

		public ContextView (object view)
		{
			_view = view;
		}

		public object view
		{
			get
			{
				return _view;
			}
		}
	}
}


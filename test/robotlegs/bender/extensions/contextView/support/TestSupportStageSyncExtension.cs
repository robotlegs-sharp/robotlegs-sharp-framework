using System;
using robotlegs.bender.extensions.contextview;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.viewManager.support;

namespace robotlegs.bender.extensions.contextView.support
{
	public class TestSupportStageSyncExtension : StageSyncExtension
	{
		public IViewStateWatcher viewStateWatcher;

		protected override IViewStateWatcher GetContextViewStateWatcher(object contextView)
		{
			if (viewStateWatcher == null)
			{
				viewStateWatcher = new TestObjectViewStateWatcher (contextView);
			}
			return viewStateWatcher;
		}
	}
}


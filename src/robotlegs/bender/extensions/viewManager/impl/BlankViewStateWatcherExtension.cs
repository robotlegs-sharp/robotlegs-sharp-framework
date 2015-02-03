using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender
{
	public class BlankViewStateWatcherExtension : ViewStateWatcherExtension
	{
		protected override IViewStateWatcher GetViewStateWatcher (object contextView)
		{
			return null;
		}
	}
}


using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportView : SupportContainer, IView
	{
		public event Action<IView> RemoveView;

		public SupportView ()
		{

		}

		public void Register()
		{
			ViewNotifier.RegisterView (this, GetType());
		}

		public void CallRemoveView()
		{
			if (RemoveView != null)
				RemoveView (this);
		}
	}
}


using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportView : SupportContainer, IView
	{
		public event Action<IView> RemoveView;

		private bool registerd = false;

		public SupportView ()
		{

		}

		public override SupportContainer Parent
		{
			get
			{
				return base.Parent;
			}
			protected set
			{
				base.Parent = value;
				if (value != null)
				{
					Register ();
				}
			}
		}

		public void Register()
		{
			if (registerd)
				return;

			registerd = true;
			ViewNotifier.RegisterView (this, GetType());
		}

		public void CallRemoveView()
		{
			if (RemoveView != null)
				RemoveView (this);
		}
	}
}

